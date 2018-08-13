using UnityEngine;
using System.Collections;

namespace CustomProjectSettings.Internal
{
    public abstract class CustomSettingsBase : ScriptableObject
    {
        public void Initialise()
        {
#if UNITY_EDITOR
            //Save this by default when first created
            CustomSettingsUtility.SaveInstance(this);
#endif
            OnInitialise();
        }

        public abstract void OnWillSave();
        protected abstract void OnInitialise();

        public void Save()
        {
            CustomSettingsUtility.SaveInstance(this);
        }
    }
}

namespace CustomProjectSettings
{
    public abstract class CustomSettings<T> : Internal.CustomSettingsBase, ISerializationCallbackReceiver where T : Internal.CustomSettingsBase
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    CustomSettingsUtility.GetInstance(ref _instance);
                }
                return _instance;
            }
        }
        private static T _instance;

#if UNITY_EDITOR
        public static void Select()
        {
            UnityEditor.Selection.activeObject = Instance;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _instance = this as T;
            CustomSettingsUtility.GetInstance(ref _instance);
        }
#endif
    }
}