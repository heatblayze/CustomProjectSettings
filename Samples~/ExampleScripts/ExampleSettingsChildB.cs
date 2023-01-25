using CustomProjectSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSettingsChildB : CustomSettingsFile<ExampleSettingsRoot>
{
    public override string Title => "Example Settings B";

    public bool Boolean => _boolean;

    [SerializeField]
    bool _boolean;
}
