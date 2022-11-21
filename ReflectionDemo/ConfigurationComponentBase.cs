using System;
using System.Configuration;
using System.Reflection;

namespace ReflectionDemo
{
    public class ConfigurationComponentBase
    {
        private readonly ConfigurationComponentBase _instance;
        private readonly Type _type;

        public ConfigurationComponentBase()
        {
            _instance = this;
            _type = _instance.GetType();
        }

        //public void UpdateSettings()
        //{
        //    var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    foreach (var prop in propInfo)
        //    {
        //        var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));

        //        if (attr == null) continue;

        //        RestorePropValueFromSettings(prop, attr);

        //        Console.WriteLine($"Restored value from settings for {prop.Name}: {prop.GetValue(_instance)}");

        //        SetNewPropValue(prop);

        //        Console.WriteLine($"The new property value for {prop.Name}: {prop.GetValue(_instance)}");

        //        SaveSettings(attr.SettingName, prop.GetValue(_instance)?.ToString());

        //        Console.WriteLine();
        //    }
        //}

        public void LoadSettings()
        {
            var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in propInfo)
            {
                var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));

                if (attr == null) continue;

                RestorePropValueFromSettings(prop, attr);
            }
        }

        public void SaveSettings()
        {
            var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in propInfo)
            {
                var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));

                if (attr == null) continue;

                SaveSettings(attr.SettingName, prop.GetValue(_instance)?.ToString());
            }
        }

        private void RestorePropValueFromSettings(PropertyInfo property, SaveToConfigAttribute attribute)
        {
            switch (property.PropertyType.Name)
            {
                case "Int32":
                    property.SetValue(_instance, int.TryParse(ReadSetting(attribute.SettingName), out var intValue) ? intValue : 0);
                    break;
                case "Single":
                    property.SetValue(_instance, float.TryParse(ReadSetting(attribute.SettingName), out var floatValue) ? floatValue : 0.0f);
                    break;
                case "String":
                    property.SetValue(_instance, ReadSetting(attribute.SettingName));
                    break;
                case "TimeSpan":
                    property.SetValue(_instance, TimeSpan.TryParse(ReadSetting(attribute.SettingName), out var timeSpan) ? timeSpan : TimeSpan.Zero);
                    break;
            }
        }

        //private void SetNewPropValue(PropertyInfo property)
        //{
        //    switch (property.PropertyType.Name)
        //    {
        //        case "Int32":
        //            Console.Write("Enter new value: ");
        //            property.SetValue(_instance, ParseInt() ?? property.GetValue(_instance));
        //            break;
        //        case "Single":
        //            Console.Write("Enter new value: ");
        //            property.SetValue(_instance, ParseFloat() ?? property.GetValue(_instance));
        //            break;
        //        case "String":
        //            Console.Write("Enter new value: ");
        //            property.SetValue(_instance, Console.ReadLine());
        //            break;
        //        case "TimeSpan":
        //            Console.Write("Enter new value: ");
        //            property.SetValue(_instance, ParseTimeSpan() ?? property.GetValue(_instance));
        //            break;
        //    }
        //}

        //private static int? ParseInt()
        //{
        //    var success = int.TryParse(Console.ReadLine(), out var result);
        //    if (!success)
        //        Console.WriteLine("You have entered not correct value for int. Property value will not change");
        //    return success ? result : (int?)null;
        //}

        //private static float? ParseFloat()
        //{
        //    var success = float.TryParse(Console.ReadLine(), out var result);
        //    if (!success)
        //        Console.WriteLine("You have entered not correct value for float. Property value will not change");
        //    return success ? result : (float?)null;
        //}

        //private static TimeSpan? ParseTimeSpan()
        //{
        //    var success = TimeSpan.TryParse(Console.ReadLine(), out var result);
        //    if (!success)
        //        Console.WriteLine("You have entered not correct value for TimeSpan. Property value will not change");
        //    return success ? result : (TimeSpan?)null;
        //}

        private static string ReadSetting(string key)
        {
            var result = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }

            return result;
        }

        private static void SaveSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
