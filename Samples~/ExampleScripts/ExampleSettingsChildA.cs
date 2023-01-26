using CustomProjectSettings;
using UnityEngine;

public class ExampleSettingsChildA : CustomSettingsChild<ExampleSettingsRoot>
{
    public override string Title => "Example Settings A";

    public string String => _string;
    [SerializeField]
    string _string;
}
