using UnityEngine;
using System.Collections;
using CustomProjectSettings;

public class TestSetting : CustomSettings<TestSetting>
{
    public bool TEST;

    public override void OnWillSave()
    {
    }

    protected override void OnInitialise()
    {

    }
}
