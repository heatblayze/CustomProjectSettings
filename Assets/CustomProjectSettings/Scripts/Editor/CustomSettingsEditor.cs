using UnityEngine;
using UnityEditor;
using System.Collections;
using CustomProjectSettings.Internal;
using UnityEditor.SceneManagement;

namespace CustomProjectSettings
{
    [CustomEditor(typeof(CustomSettingsBase), true)]
    public class CustomSettingsEditor : Editor
    {
        CustomSettingsBase myTarg;

        private void OnEnable()
        {
            //Store a reference to the settings file
            myTarg = (CustomSettingsBase)target;

            //Hook into the undo performed function
            Undo.undoRedoPerformed += OnUndo;
            ProjectSave.onSave += OnSave;
        }

        private void OnDisable()
        {
            //Remove the hook when not visible. Unity handles everything, don't worry!
            Undo.undoRedoPerformed -= OnUndo;
            ProjectSave.onSave -= OnSave;
        }

        public override void OnInspectorGUI()
        {
            //Listen for changes
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (scope.changed)
                {
                    //Save the file on change
                    //myTarg.Save();
                }
            }
        }

        void OnUndo()
        {
            //When Unity performs an Undo, it automatically reverts the changes.
            //So all we need to do is save the file again
            //myTarg.Save();
        }

        void OnSave()
        {
            myTarg.Save();
        }
    }

    public class ProjectSave : UnityEditor.AssetModificationProcessor
    {
        public static event System.Action onSave;

        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (onSave != null)
                onSave.Invoke();
            return paths;
        }
    }
}