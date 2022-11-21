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

            UpdateSettings();
        }

        public void UpdateSettings()
        {
            var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in propInfo)
            {
                var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));
                if (attr != null)
                {
                    var propType = prop.PropertyType;
                    ReadSetting(attr.SettingName);
                    Console.WriteLine("Enter New Value:");
                    var newValue = int.Parse(Console.ReadLine() ?? "");

                    prop.SetValue(_instance, newValue);
                    SaveProperties(attr.SettingName, prop.GetValue(_instance).ToString());
                }
            }
        }

        //public void UpdateSettings()
        //{
        //    var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    foreach (var prop in propInfo)
        //    {
        //        var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));
        //        if (attr != null)
        //        {
        //            ReadSetting(attr.SettingName);
        //            Console.WriteLine("Enter New Value:");
        //            var newValue = int.Parse(Console.ReadLine() ?? "");

        //            prop.SetValue(_instance, newValue);
        //            SaveProperties(attr.SettingName, prop.GetValue(_instance).ToString());
        //        }
        //    }
        //}

        //public void SaveProps()
        //{
        //    var propInfo = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    foreach (var prop in propInfo)
        //    {
        //        var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));
        //        if (attr != null)
        //        {
        //            SaveProperties(attr.SettingName, prop.GetValue(_instance).ToString());
        //        }
        //    }
        //}

        static void ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                Console.WriteLine($"{key} = {result}");
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        static void SaveProperties(string key, string value)
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