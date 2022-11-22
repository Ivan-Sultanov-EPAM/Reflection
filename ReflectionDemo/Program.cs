using System;

namespace ReflectionDemo
{
    internal class Program
    {
        private static void Main()
        {
            var demo = new CustomAttributeDemo();
            ShowProps(demo);

            Console.WriteLine("Loading values from settings...");
            Console.WriteLine();
            demo.LoadSettings();

            ShowProps(demo);

            Console.WriteLine("To modify props values enter 1");
            if (Console.ReadKey().KeyChar == '1')
            {
                Console.WriteLine();
                SetNewPropValue(demo);
                Console.WriteLine("Saving Settings...");
                demo.SaveSettings();

                Console.WriteLine();
                ShowProps(demo);
            }

            Console.WriteLine();
            Console.WriteLine("Restart program to see if property values saved");
        }

        private static void ShowProps(CustomAttributeDemo demo)
        {
            Console.WriteLine("Properties values:");
            Console.WriteLine($"\tIntProp: {demo.IntProp}");
            Console.WriteLine($"\tFloatProp: {demo.FloatProp}");
            Console.WriteLine($"\tStringProp: {demo.StringProp}");
            Console.WriteLine($"\tTimeSpanProp: {demo.TimeSpanProp}");
            Console.WriteLine();
        }

        private static void SetNewPropValue(CustomAttributeDemo demo)
        {
            var propInfo = demo.GetType().GetProperties();
            foreach (var property in propInfo)
            {
                switch (property.PropertyType.Name)
                {
                    case "Int32":
                        Console.Write($"Enter new value for {property.Name}: ");
                        property.SetValue(demo, ParseInt() ?? property.GetValue(demo));
                        break;
                    case "Single":
                        Console.Write($"Enter new value for {property.Name}: ");
                        property.SetValue(demo, ParseFloat() ?? property.GetValue(demo));
                        break;
                    case "String":
                        Console.Write($"Enter new value for {property.Name}: ");
                        property.SetValue(demo, Console.ReadLine());
                        break;
                    case "TimeSpan":
                        Console.Write($"Enter new value for {property.Name}: ");
                        property.SetValue(demo, ParseTimeSpan() ?? property.GetValue(demo));
                        break;
                }
            }

        }

        private static int? ParseInt()
        {
            var success = int.TryParse(Console.ReadLine(), out var result);
            if (!success)
                Console.WriteLine("You have entered not correct value for int. Property value will not change");
            return success ? result : (int?)null;
        }

        private static float? ParseFloat()
        {
            var success = float.TryParse(Console.ReadLine(), out var result);
            if (!success)
                Console.WriteLine("You have entered not correct value for float. Property value will not change");
            return success ? result : (float?)null;
        }

        private static TimeSpan? ParseTimeSpan()
        {
            var success = TimeSpan.TryParse(Console.ReadLine(), out var result);
            if (!success)
                Console.WriteLine("You have entered not correct value for TimeSpan. Property value will not change");
            return success ? result : (TimeSpan?)null;
        }
    }
}
