# CustomProjectSettings
Custom Project Settings files for Unity

I wanted some custom settings for my Unity project, so I made some simple functionality to allow for this.

Essentially this allows you to create a ScriptableObject that is saved outside of the "Assets" folder in your project, that can then be edited in the Inspector in the Unity Editor.

These files are automatically copied to the folder "Resources/CustomSettings" at build time and removed on completion of the build.

## How to create your own settings file
Here's a sample for creating a settings file class:

```c#
using CustomProjectSettings;

public class ExampleSetting : CustomSettings<ExampleSetting>
{
    public bool DemoValue;

    public override void OnWillSave() { }
    protected override void OnInitialise() { }
}
```

## How to access the settings
To access the settings file, you just need to create a method hooked up to a MenuItem, then call Select() on the class:
**Ensure to place this script inside and "Editor/" folder, or use #if UNITY_EDITOR**
```c#
using UnityEditor;

public class SettingsDemoMenu
{
    [MenuItem("Edit/Custom Project Settings/Example Settings")]
    public static void ShowInspector()
    {
        ExampleSetting.Select();
    }
}
```

## Want custom inspector control?
All CustomSettings will have a default inspector that enables saving & undo functionality

If you'd like to create your own, just read [CustomSettingsEditor.cs](/Assets/CustomProjectSettings/Scripts/Editor/CustomSettingsEditor.cs)
