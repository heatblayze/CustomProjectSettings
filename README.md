# Custom Project Settings
Enables custom, global, scriptable settings for the Unity Editor that are readable in builds.

---

## What does this do?
This small set of code allows you to create a [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) that is saved in the  _Assets/Resources/Settings_. folder of your project, which you can use to store global information about your project and retrieve it anywhere at runtime.

## Install
There are two methods for installing the package.

**It is highly recommended that you specify a version to avoid pulling from the repo directly.**

### 1. Via Unity Package Manager
Open the Unity Package Manager, click the "+" button located at the top-left and select "Install via Git URL". Then paste the following code (replacing the version number as required):

`https://github.com/heatblayze/CustomProjectSettings.git#2.0.2`

### 2. Directly into the manifest file
Open the package manifest file located at _Packages/manifest.json_ (starting from the project root directory, NOT the Assets folder).

Add the following line at the top of your dependencies (replacing the version number as required):

`"com.heatblayze.customprojectsettings": "https://github.com/heatblayze/CustomProjectSettings.git#2.0.2",`

It should look something like this:
```
{
  "dependencies": {
    "com.heatblayze.customprojectsettings": "https://github.com/heatblayze/CustomProjectSettings.git#2.0.2",
    ...
  },
}
```

## How to create your own settings file
### Create the root settings class
This is just a `ScriptableObject`, as such you should place it in a file that shares the same name as the class.
```c#
using UnityEngine;
using CustomProjectSettings;

public class ExampleSettingsRoot : CustomSettingsRoot
{
    public override string Title => "Example Settings";

    public float Float => _float;
    [SerializeField]
    float _float = 1f;
}
```

### Create the settings descriptor class
This class can be in the same file as the root, or wherever you please. Simply inherit from `CustomSettingsRootDescriptor<T>`, specifying your `CustomSettingsRoot` class from above.
```c#
using CustomProjectSettings;

public class ExampleSettingsDescriptor : CustomSettingsRootDescriptor<ExampleSettingsRoot>
{
    public override string Filename => "ExampleSettings";
}
```

### Creating child settings
Simply inherit from `CustomSettingsChild<T>`, specifying the `CustomSettingsRoot` it is linked to.
This is also a `ScriptableObject`, so it must be placed in a file that shares the name of the class.
```c#
using CustomProjectSettings;
using UnityEngine;

public class ExampleSettingsChildA : CustomSettingsChild<ExampleSettingsRoot>
{
    public override string Title => "Example Settings A";

    public string String => _string;
    [SerializeField]
    string _string;
}
```

## How to access the settings
Simply call `CustomSettings.GetSettings<T>()` like shown below.

```c#
using UnityEngine;
using CustomProjectSettings;

public class ExampleSettingsLogger : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"Example root float: {CustomSettings.GetSettings<ExampleSettingsRoot>().Float}");
        Debug.Log($"Example child A string: {CustomSettings.GetSettings<ExampleSettingsChildA>().String}");
    }
}
```

## Want custom inspector control?
You have it!

Inspectors use a default editor, simply to remove the "Script" field that shows on `ScriptableObject`s, but this will be overwritten if you specify an Editor for your settings class(es). If you wish to view the source of the default inspector, it can be found at _Editor/CustomSettingsEditor.cs_

Custom `PropertyDrawer`s should also work as usual.
