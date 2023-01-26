using CustomProjectSettings;
using UnityEngine;

public class ExampleSettingsChildB : CustomSettingsChild<ExampleSettingsRoot>
{
    public override string Title => "Example Settings B";

    public bool Boolean => _boolean;
    [SerializeField]
    bool _boolean;
}
