using CustomProjectSettings.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomProjectSettings.Editor
{
    public class CustomSettingsProvider : SettingsProvider
    {
        private CustomSettingsFile _customSettings;
        private Type _childType;
        private Type _rootType;
        private UnityEditor.Editor _cachedEditor;

        // Register the SettingsProvider
        [SettingsProviderGroup]
        public static SettingsProvider[] CreateSettingsProviders()
        {
            List<SettingsProvider> providers = new List<SettingsProvider>();
            foreach (var rootType in CustomSettingsTypeCache.SettingsRootTypes)
            {
                var settingsAsset = CustomSettingsGenerator.GetOrCreateSettings(rootType);
                var rootProvider = new CustomSettingsProvider(rootType, null, $"Project/{settingsAsset.Title}", SettingsScope.Project);
                providers.Add(rootProvider);

                foreach (var item in settingsAsset.Children)
                {
                    if (item != null)
                    {
                        var provider = new CustomSettingsProvider(rootType, item.GetType(), $"Project/{settingsAsset.Title}/{item.Title}", SettingsScope.Project);
                        provider.keywords = GetSearchKeywordsFromSerializedObject(new SerializedObject(item));
                        providers.Add(provider);
                    }
                }
            }

            return providers.ToArray();
        }

        public CustomSettingsProvider(Type rootType, Type childType, string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope)
        {
            this._rootType = rootType;
            this._childType = childType;
        }

        // This function is called when the user clicks on the MyCustom element in the Settings window.
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _customSettings = CustomSettingsGenerator.GetOrCreateSettings(_rootType);
            if (_childType != null)
                _customSettings = (_customSettings as CustomSettingsRoot).Children.First(x => x.GetType() == _childType);
        }

        public override void OnGUI(string searchContext)
        {
            ++EditorGUI.indentLevel;
            EditorGUILayout.Space();

            var w = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 259;

            // TODO: Allow for custom editor types
            UnityEditor.Editor.CreateCachedEditor(_customSettings, null, ref _cachedEditor);
            _cachedEditor.OnInspectorGUI();

            EditorGUIUtility.labelWidth = w;

            --EditorGUI.indentLevel;
        }
    }
}