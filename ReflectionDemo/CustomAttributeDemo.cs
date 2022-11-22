using System;

namespace ReflectionDemo
{
    public class CustomAttributeDemo : ConfigurationComponentBase
    {
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