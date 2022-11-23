using System;

namespace ReflectionDemo
{
    public class CustomAttributeDemo : ConfigurationComponentBase
    {
        [SaveToConfig("TestInt", SettingsProvider.ConfigurationManager)]
        public int IntProp { get; set; }
        [SaveToConfig("TestFloat", SettingsProvider.ConfigurationManager)]
        public float FloatProp { get; set; }
        [SaveToConfig("TestString", SettingsProvider.ConfigurationManager)]
        public string StringProp { get; set; }
        [SaveToConfig("TestTimeSpan", SettingsProvider.ConfigurationManager)]
        public TimeSpan TimeSpanProp { get; set; }
    }
}