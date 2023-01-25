using CustomProjectSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;

namespace CustomProjectSettings
{
    public static class CustomSettings
    {
        public static T GetSettings<T>() where T : Internal.CustomSettingsFile
        {
            var type = typeof(T);
            if (typeof(CustomSettingsRoot).IsAssignableFrom(type)) // Retrieving the root file
            {
                return LoadRoot(type) as T;
            }

            var rootType = Internal.CustomSettingsTypeCache.GetRootType(type);
            return LoadRoot(rootType).Children.First(x => x.GetType() == type) as T;
        }

        static CustomSettingsRoot LoadRoot(Type type)
        {
            var descriptor = Internal.CustomSettingsTypeCache.GetDescriptor(type);
            if (descriptor == null) return null;
            return Resources.Load<CustomSettingsRoot>(Internal.CustomSettingsRootDescriptor.GetRuntimePath(descriptor));
        }
    }
}