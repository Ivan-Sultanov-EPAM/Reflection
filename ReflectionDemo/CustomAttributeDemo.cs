using System;

namespace ReflectionDemo
{
    public class CustomAttributeDemo : ConfigurationComponentBase
    {
        [SaveToConfig("TestInt")]
        public int IntProp { get; set; }
        public float FloatProp { get; set; }
        public string StringProp { get; set; }
        public TimeSpan TimeSpanProp { get; set; }

        public void Demonstrate()
        {
            //ReadProps();
            //Console.WriteLine("Properties values:");
            //Console.WriteLine($"IntProp: {IntProp}");
            //Console.WriteLine($"FloatProp: {FloatProp}");
            //Console.WriteLine($"StringProp: {StringProp}");
            //Console.WriteLine($"TimeSpanProp: {TimeSpanProp}");
            //Console.WriteLine();

            //Console.WriteLine("Enter value for Int:");
            //IntProp = int.Parse(Console.ReadLine() ?? "");

            //Console.WriteLine("Enter value for Float:");
            //FloatProp = float.Parse(Console.ReadLine() ?? "");

            //Console.WriteLine("Enter value for String:");
            //StringProp = Console.ReadLine();

            //Console.WriteLine("Enter value for TimeSpan:");
            //TimeSpanProp = TimeSpan.Parse(Console.ReadLine() ?? "");

            //Console.WriteLine();

            //SaveProps();

            //Console.WriteLine("Properties new values:");
            //Console.WriteLine($"IntProp: {IntProp}");
            //Console.WriteLine($"FloatProp: {FloatProp}");
            //Console.WriteLine($"StringProp: {StringProp}");
            //Console.WriteLine($"TimeSpanProp: {TimeSpanProp}");
        }
    }
}