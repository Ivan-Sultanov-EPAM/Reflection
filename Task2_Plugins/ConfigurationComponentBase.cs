using System;
using System.Collections.Generic;
using System.Reflection;

namespace Task2_Plugins
{
    public class ConfigurationComponentBase
    {
        private readonly ConfigurationComponentBase _instance;
        private readonly Type _type;
        private Dictionary<string, IConfigurationProvider> _providers;

        public ConfigurationComponentBase(string pluginsPath)
        {
            _instance = this;
            _type = _instance.GetType();
            _providers = PluginLoader.LoadPlugins(pluginsPath);
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

                switch (attr.ProviderType)
                {
                    case SettingsProvider.File:
                        _providers["FileConfigurationProvider"].SaveSetting(attr.SettingName, prop.GetValue(_instance)?.ToString());
                        break;
                    case SettingsProvider.ConfigurationManager:
                        _providers["ConfigurationManagerConfigurationProvider"].SaveSetting(attr.SettingName, prop.GetValue(_instance)?.ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(SettingsProvider));
                }
            }
        }

        private void RestorePropValueFromSettings(PropertyInfo property, SaveToConfigAttribute attribute)
        {
            Func<string, string> readSettingAction = attribute.ProviderType switch
            {
                SettingsProvider.File => (x) => _providers["FileConfigurationProvider"].ReadSetting(x),
                SettingsProvider.ConfigurationManager => (x) => _providers["ConfigurationManagerConfigurationProvider"].ReadSetting(x),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute))
            };

            switch (property.PropertyType.Name)
            {
                case "Int32":
                    property.SetValue(_instance, int.TryParse(readSettingAction(attribute.SettingName), out var intValue) ? intValue : 0);
                    break;
                case "Single":
                    property.SetValue(_instance, float.TryParse(readSettingAction(attribute.SettingName), out var floatValue) ? floatValue : 0.0f);
                    break;
                case "String":
                    property.SetValue(_instance, readSettingAction(attribute.SettingName));
                    break;
                case "TimeSpan":
                    property.SetValue(_instance, TimeSpan.TryParse(readSettingAction(attribute.SettingName), out var timeSpan) ? timeSpan : TimeSpan.Zero);
                    break;
            }
        }
    }
}
