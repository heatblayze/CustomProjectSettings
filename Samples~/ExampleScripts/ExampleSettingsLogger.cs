using UnityEngine;
using CustomProjectSettings;

public class ExampleSettingsLogger : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"Example root float: {CustomSettings.GetSettings<ExampleSettingsRoot>().Float}");
        Debug.Log($"Example child A string: {CustomSettings.GetSettings<ExampleSettingsChildA>().String}");
        Debug.Log($"Example child B boolean: {CustomSettings.GetSettings<ExampleSettingsChildB>().Boolean}");
    }
}
