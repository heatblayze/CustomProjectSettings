using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoader : MonoBehaviour
{
    public Text uiText;

    // Use this for initialization
    void Start()
    {
        string text =
@"Example settings loaded
TEST: {0}
SampleTextInput: {1}
DemoClass:
    name: {2}
    someValue: {3}";

        //Nice long format eeeep
        uiText.text = string.Format(text, ExampleSetting.Instance.TEST, ExampleSetting.Instance.SampleTextInput,
            ExampleSetting.Instance.SampleClass.name, ExampleSetting.Instance.SampleClass.someValue);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {

    }
}
