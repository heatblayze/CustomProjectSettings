using UnityEditor;

public class SettingsDemoMenu
{
    [MenuItem("Edit/Custom Project Settings/Example Settings")]
    public static void ShowInspector()
    {
        ExampleSetting.Select();
    }
}