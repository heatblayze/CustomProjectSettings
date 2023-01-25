using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CustomProjectSettings.Internal
{
    public static class CustomSettingsTypeCache
    {
        public static ReadOnlyCollection<Type> SettingsRootTypes
        {
            get
            {
                if (s_roots == null) Init();
                return s_roots.AsReadOnly();
            }
        }

        static Dictionary<Type, Type> s_settingsRootDescriptors;
        static Dictionary<Type, Type> s_settingsChildRoots;
        static Dictionary<Type, List<Type>> s_settingsRootChildren;
        static List<Type> s_roots;
        static List<CustomSettingsRootDescriptor> s_descriptors;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            s_settingsRootDescriptors = GetTypesWithConstraint(typeof(CustomSettingsRootDescriptor), typeof(CustomSettingsRoot)).KeyArgToType();
            s_descriptors = s_settingsRootDescriptors.Values.Select(t => (CustomSettingsRootDescriptor)Activator.CreateInstance(t)).ToList();

            var settingsFiles = GetTypesWithConstraint(typeof(CustomSettingsFile), typeof(CustomSettingsRoot));
            s_settingsChildRoots = settingsFiles.KeyArgToType(invert: true);
            s_settingsRootChildren = settingsFiles.CreateCollection();

            s_roots = GetTypes(typeof(CustomSettingsRoot));
        }

        static List<Type> GetTypes(Type rootType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes())
                .Where(type =>
                    rootType.IsAssignableFrom(type)
                    && !type.IsAbstract
                    && !type.IsGenericType)
                .ToList();
        }

        static IEnumerable<Type> GetTypesWithConstraint(Type valueType, Type constraintType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes())
                .Where(type =>
                    valueType.IsAssignableFrom(type)
                    && !type.IsAbstract
                    && !type.IsGenericType
                    && type.BaseType.IsGenericType
                    && constraintType.IsAssignableFrom(type.BaseType.GetGenericArguments()[0])
                    && !type.BaseType.GetGenericArguments()[0].IsAbstract);
        }

        static Dictionary<Type, Type> KeyArgToType(this IEnumerable<Type> types, bool invert = false)
        {
            return types.ToDictionary(
                type => !invert ? type.BaseType.GetGenericArguments()[0] : type,
                type => !invert ? type : type.BaseType.GetGenericArguments()[0]);
        }

        static Dictionary<Type, List<Type>> CreateCollection(this IEnumerable<Type> types)
        {
            var dictionary = new Dictionary<Type, List<Type>>();
            foreach (var type in types)
            {
                var key = type.BaseType.GetGenericArguments()[0];
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, new List<Type> { type });
                else
                    dictionary[key].Add(type);
            }
            return dictionary;
        }

        public static CustomSettingsRootDescriptor GetDescriptor(Type settingsType)
        {
            if(s_settingsRootDescriptors == null) Init();
            if(s_settingsRootDescriptors.TryGetValue(settingsType, out var descriptorType))
            {
                return s_descriptors.First(d => d.GetType() == descriptorType);
            }
            return null;
        }

        public static Type GetRootType(Type settingsType)
        {
            if (s_settingsChildRoots == null) Init();
            if (s_settingsChildRoots.TryGetValue(settingsType, out var rootType)) return rootType;
            return null;
        }

        public static List<Type> GetSettingsTypes(Type rootType)
        {
            if (s_settingsRootChildren == null) Init();
            if (s_settingsRootChildren.TryGetValue(rootType, out var settingsTypes)) return settingsTypes;
            return new List<Type>();
        }
    }
}