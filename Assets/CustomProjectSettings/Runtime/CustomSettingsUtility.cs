using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CustomProjectSettings.Internal;

namespace CustomProjectSettings
{
    public static class CustomSettingsUtility
    {
        public static void GetInstance<T>(ref T settings) where T : CustomSettingsBase
        {
#if UNITY_EDITOR
            if (settings != null)
            {
                if (!ProjectSave.HasReference(settings.Save))
                    ProjectSave.onSave += settings.Save;
                return;
            }
#endif

            string fileName = typeof(T).ToString();
#if UNITY_EDITOR
            fileName = fileName + ".json";
            string folder = Directory.GetParent(Application.dataPath) + "/CustomProjectSettings/";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (File.Exists(folder + fileName))
            {
                try
                {
                    settings = (T)ScriptableObject.CreateInstance(typeof(T));
                    //Attempt to read the file and overwrite the contents of the ScriptableObject
                    JsonUtility.FromJsonOverwrite(File.ReadAllText(folder + fileName), settings);

                    if (!ProjectSave.HasReference(settings.Save))
                        ProjectSave.onSave += settings.Save;
                    return;
                }
                catch (Exception e)
                {
                    //Something went wrong reading the file
                    string result = "Failed to read type of {0} from {1} : {2}";
                    Debug.LogWarning(string.Format(result, typeof(T), folder + fileName, e.Message));
                }
            }

            Debug.Log(string.Format("Creating new instance of {0}", typeof(T)));

            //First create an instance of the ScriptableObject
            settings = (T)ScriptableObject.CreateInstance(typeof(T));
            //Initialise the object
            settings.Initialise();

            if (!ProjectSave.HasReference(settings.Save))
                ProjectSave.onSave += settings.Save;

            //Weeeeeeeeee
            return;
#else
        Debug.Log("Loading setting " + fileName + " from Resources");
        try
        {
            //Load the file as a text asset
            var txtAsset = Resources.Load<TextAsset>("CustomSettings/" + fileName);
            
            settings = (T)ScriptableObject.CreateInstance(typeof(T));
            //Attempt to read the file and overwrite the contents of the ScriptableObject
            JsonUtility.FromJsonOverwrite(txtAsset.text, settings);
            return;
        }
        catch
        {
            Debug.LogWarning(string.Format("Failed to read custom setting {0}. Creating default instance.", fileName));

            settings = (T)ScriptableObject.CreateInstance(typeof(T));
            settings.Initialise();
            return;
        }
#endif
        }

        public static void SaveInstance(CustomSettingsBase settings)
        {
#if UNITY_EDITOR
            string folder = Directory.GetParent(Application.dataPath) + "/CustomProjectSettings/";
            string fileName = settings.GetType() + ".json";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            //Inform the settings it's about to be saved
            settings.OnWillSave();
            try
            {
                //Attempt to write text
                File.WriteAllText(folder + fileName, JsonUtility.ToJson(settings, true));
            }
            catch (Exception e)
            {
                //Something went wrong saving the file
                string result = "Failed to save type of {0} to {1} : {2}";
                Debug.LogError(string.Format(result, settings.GetType(), folder + fileName, e.Message));
            }
#else
        Debug.LogWarning("Cannot save project settings at runtime");
#endif
        }
    }

#if UNITY_EDITOR
    public class ProjectSave : UnityEditor.AssetModificationProcessor
    {
        public static event Action onSave;

        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (onSave != null)
                onSave.Invoke();
            return paths;
        }

        public static bool HasReference(Action action)
        {
            if (onSave == null)
                return false;

            var list = onSave.GetInvocationList();
            foreach (var item in list)
            {
                if (item.Target == action.Target && item.Method == action.Method)
                    return true;
            }
            return false;
        }
    }
#endif
}