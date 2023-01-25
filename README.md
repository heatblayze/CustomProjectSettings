# NOTE: THIS PROJECT IS UNDERGOING REVIVAL
**If you wish to use the current release version of this package, please ensure you specify the version number when installing.**

The information below is for the current 1.x version of the package. This will not be backwards compatible with version 2.0, which will be released soon™ (date at writing is 2023/01/26).

This document will be updated to reflect these changes, and there will also be an example bundled with the package.

---

# Custom Project Settings
Enables custom, global, scriptable settings for the Unity Editor that are readable in builds

## What does this do?
This small set of code allows you to create a `ScriptableObject` that is saved outside of the _Assets_ folder in your project, that can then be edited in the Inspector in the Unity Editor.

## Where are the files stored?
The settings files are stored as .json files, in a folder called _CustomSettings_, a sibling folder of _Assets_ and _ProjectSettings_

These settings files are automatically copied to the folder _Resources/CustomSettings_ at build time and removed on completion of the build.

## Install
Either download the latest .unitypackage via the Releases tab OR
Add the package to your dependencies in the manifest.json (the #1.3.0 being the version)
```
{
  "dependencies": {
    "com.heatblayze.customprojectsettings": "https://github.com/heatblayze/CustomProjectSettings.git#1.3.0",
    ...
  },
}
```

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
To access the settings file, you just need to create a method hooked up to a MenuItem, then call `MySettings.Select()` on the class:
**Ensure to place this script inside an _Editor/_ folder, or use _#if UNITY_EDITOR_**
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
You have it! Inspectors are not overridden in my code.
