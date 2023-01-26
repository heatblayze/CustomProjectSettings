using CustomProjectSettings;
using UnityEngine;

public class ExampleSettingsChildA : CustomSettingsFile<ExampleSettingsRoot>
{
    public override string Title => "Example Settings A";

    public string String => _string;
    [SerializeField]
    string _string;
}
