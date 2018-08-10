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
            myTarg = (CustomSettingsBase)target;
            Undo.undoRedoPerformed += OnUndo;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndo;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                myTarg.Save();
            }
        }

        void OnUndo()
        {
            myTarg.Save();
        }
    }
}