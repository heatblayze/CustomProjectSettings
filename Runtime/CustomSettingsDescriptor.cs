namespace CustomProjectSettings.Internal
{
    public abstract class CustomSettingsRootDescriptor
    {
        public const string ResourcesRoot = "Assets/Resources/";
        public const string SettingsFolder = "Settings/";
        public abstract string Filename { get; }

        public static string GetEditorPath(CustomSettingsRootDescriptor descriptor) => ResourcesRoot + SettingsFolder + descriptor.Filename + ".asset";
        public static string GetRuntimePath(CustomSettingsRootDescriptor descriptor) => SettingsFolder + descriptor.Filename;
    }
}

namespace CustomProjectSettings
{
    public abstract class CustomSettingsRootDescriptor<T> : Internal.CustomSettingsRootDescriptor where T : CustomSettingsRoot { }
}