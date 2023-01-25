using CustomProjectSettings.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CustomProjectSettings.Editor
{
    public static class CustomSettingsGenerator
    {
        internal static CustomSettingsRoot GetOrCreateSettings(Type rootType)
        {
            var descriptor = CustomSettingsTypeCache.GetDescriptor(rootType);
            if (descriptor == null) return null;

            var filePath = CustomSettingsRootDescriptor.GetEditorPath(descriptor);

            // Find or create the actual asset file
            var settingsAsset = (CustomSettingsRoot)AssetDatabase.LoadAssetAtPath(filePath, rootType);
            if (settingsAsset == null)
            {
                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                AssetDatabase.Refresh();

                settingsAsset = (CustomSettingsRoot)ScriptableObject.CreateInstance(rootType);
                AssetDatabase.CreateAsset(settingsAsset, filePath);
            }

            // Check for any null entries, remove them, then re-create the settings file
            // Null entries will usually occur when the class for the entry is unavailable

            // Get access to the children of the asset
            var childrenFI = rootType.GetField("_children", BindingFlags.Instance | BindingFlags.NonPublic);
            var settingsChildren = (List<CustomSettingsFile>)childrenFI.GetValue(settingsAsset);

            // Collect the types of all the current settings
            bool hasNull = false;
            List<Type> currentTypes = new List<Type>();
            for (int i = 0; i < settingsChildren.Count; i++)
            {
                if (settingsChildren[i] != null)
                    currentTypes.Add(settingsChildren[i].GetType());
                else
                {
                    hasNull = true;
                    settingsChildren.RemoveAt(i);
                    --i;
                }
            }

            if (hasNull)
            {
                // Create clones of the current settings
                List<CustomSettingsFile> settings = new List<CustomSettingsFile>();
                for (int i = 0; i < settingsChildren.Count; i++)
                {
                    // Don't need to null check because they were just removed
                    var json = JsonUtility.ToJson(settingsChildren[i]);
                    var item = ScriptableObject.CreateInstance(settingsChildren[i].GetType());
                    JsonUtility.FromJsonOverwrite(json, item);
                    settings.Add((CustomSettingsFile)item);
                }

                // Delete the existing asset
                AssetDatabase.DeleteAsset(filePath);

                // Create a new one
                settingsAsset = (CustomSettingsRoot)ScriptableObject.CreateInstance(rootType);
                AssetDatabase.CreateAsset(settingsAsset, filePath);

                // Copy the old settings into the new asset
                foreach (var item in settings)
                {
                    settingsChildren.Add(item);
                    AssetDatabase.AddObjectToAsset(item, filePath);
                }
            }

            foreach (var childTypes in CustomSettingsTypeCache.GetSettingsTypes(rootType).Where(t => !settingsChildren.Any(set => set.GetType() == t)))
            {
                // Create them and add them to the settings asset
                var obj = ScriptableObject.CreateInstance(childTypes);
                var item = (CustomSettingsFile)obj;
                item.name = item.Title;
                settingsChildren.Add(item);
                AssetDatabase.AddObjectToAsset(obj, filePath);
            }
            AssetDatabase.SaveAssets();
            return settingsAsset;
        }
    }
}
