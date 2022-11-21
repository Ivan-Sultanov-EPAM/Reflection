using System;

namespace ReflectionDemo
{
    public class CustomAttributeDemo : ConfigurationComponentBase
    {
        [SaveToConfig("TestInt")]
        public int IntProp { get; set; }
        [SaveToConfig("TestFloat")]
        public float FloatProp { get; set; }
        [SaveToConfig("TestString")]
        public string StringProp { get; set; }
        [SaveToConfig("TestTimeSpan")]
        public TimeSpan TimeSpanProp { get; set; }
    }
}