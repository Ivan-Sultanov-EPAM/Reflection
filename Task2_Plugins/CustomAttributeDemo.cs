using System;

namespace Task2_Plugins
{
    public class CustomAttributeDemo : ConfigurationComponentBase
    {
        public CustomAttributeDemo(string path) :
            base(path)
        {

        }

        [SaveToConfig("TestInt", SettingsProvider.File)]
        public int IntProp { get; set; }
        [SaveToConfig("TestFloat", SettingsProvider.File)]
        public float FloatProp { get; set; }
        [SaveToConfig("TestString", SettingsProvider.File)]
        public string StringProp { get; set; }
        [SaveToConfig("TestTimeSpan", SettingsProvider.File)]
        public TimeSpan TimeSpanProp { get; set; }
    }
}