using CustomProjectSettings;

public class ExampleSetting : CustomSettings<ExampleSetting>
{
    public bool TEST;

    public override void OnWillSave() { }
    protected override void OnInitialise() { }
}