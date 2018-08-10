using UnityEngine;
using UnityEditor;

public class SettingsDemoMenu
{
    [MenuItem("Edit/Custom Project Settings/Test Settings")]
    public static void ShowInspector()
    {
        TestSetting.Select();
    }
}
