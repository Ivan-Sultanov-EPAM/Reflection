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

        public void LoadSettings()
        {
            var propInfo = _type.GetProperties();

            foreach (var prop in propInfo)
            {
                var attr = (SaveToConfigAttribute)Attribute.GetCustomAttribute(prop, typeof(SaveToConfigAttribute));

                if (attr == null) continue;

                RestorePropValueFromSettings(prop, attr);
            }
        }

        public void SaveSettings()
        {
            var propInfo = _type.GetProperties();

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
