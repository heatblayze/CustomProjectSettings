using CustomProjectSettings;

public class ExampleSetting : CustomSettings<ExampleSetting>
{
    [System.Serializable]
    public class DemoClass
    {
        public string name;
        public int someValue;
    }

    public bool TEST;
    public string SampleTextInput;
    public DemoClass SampleClass;


    public override void OnWillSave() { }
    protected override void OnInitialise() { }
}