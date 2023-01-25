using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSettingsLogger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Example root float: {CustomProjectSettings.CustomSettings.GetSettings<ExampleSettingsRoot>().Float}");
        Debug.Log($"Example child A string: {CustomProjectSettings.CustomSettings.GetSettings<ExampleSettingsChildA>().String}");
        Debug.Log($"Example child B boolean: {CustomProjectSettings.CustomSettings.GetSettings<ExampleSettingsChildB>().Boolean}");
    }
}
