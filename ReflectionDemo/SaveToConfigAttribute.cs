using System;

namespace ReflectionDemo
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SaveToConfigAttribute : Attribute
    {
        public string SettingName { get; }
        public SettingsProvider ProviderType { get; }

        public SaveToConfigAttribute(string settingName, SettingsProvider providerType)
        {
            SettingName = settingName;
            ProviderType = providerType;
        }
    }

    public enum SettingsProvider
    {
        File,
        ConfigurationManager
    }
}