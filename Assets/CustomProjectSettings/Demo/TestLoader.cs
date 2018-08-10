using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoader : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.LogError("Loading test settings. Test value is: " + ExampleSetting.Instance.TEST);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
