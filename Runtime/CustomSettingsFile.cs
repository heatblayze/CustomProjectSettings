using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace CustomProjectSettings.Internal
{
    public abstract class CustomSettingsFile : ScriptableObject
    {
        public abstract string Title { get; }
    }
}

namespace CustomProjectSettings
{
    public abstract class CustomSettingsRoot : Internal.CustomSettingsFile
    {
        public ReadOnlyCollection<Internal.CustomSettingsFile> Children => _children.AsReadOnly();
        [SerializeField, HideInInspector]
        protected List<Internal.CustomSettingsFile> _children = new List<Internal.CustomSettingsFile>();
    }

    public abstract class CustomSettingsFile<T> : Internal.CustomSettingsFile where T : CustomSettingsRoot { }
}