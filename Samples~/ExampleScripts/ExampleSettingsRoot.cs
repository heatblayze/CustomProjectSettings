using UnityEngine;
using CustomProjectSettings;

public class ExampleSettingsRoot : CustomSettingsRoot
{
    public override string Title => "Example Settings";

    public float Float => _float;
    [SerializeField]
    float _float = 1f;
}
