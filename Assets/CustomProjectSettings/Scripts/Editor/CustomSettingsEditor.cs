//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using CustomProjectSettings.Internal;
//using UnityEditor.SceneManagement;

//namespace CustomProjectSettings
//{
//    [CustomEditor(typeof(CustomSettingsBase), true)]
//    public class CustomSettingsEditor : Editor
//    {
//        CustomSettingsBase myTarg;

//        private void OnEnable()
//        {
//            //Store a reference to the settings file
//            myTarg = (CustomSettingsBase)target;

//            //Hook into the editor save functionality
//            ProjectSave.onSave += OnSave;
//        }

//        private void OnDisable()
//        {
//            ProjectSave.onSave -= OnSave;
//        }

//        void OnSave()
//        {
//            myTarg.Save();
//        }
//    }
//}