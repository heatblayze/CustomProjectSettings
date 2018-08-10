using UnityEngine;
using UnityEditor;
using System.Collections;
using CustomProjectSettings.Internal;

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
        }

        private void OnDisable()
        {
            //Remove the hook when not visible. Unity handles everything, don't worry!
            Undo.undoRedoPerformed -= OnUndo;
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
                    myTarg.Save();
                }
            }
        }

        void OnUndo()
        {
            //When Unity performs an Undo, it automatically reverts the changes.
            //So all we need to do is save the file again
            myTarg.Save();
        }
    }
}