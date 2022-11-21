using System;

namespace ReflectionDemo
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SaveToConfigAttribute : Attribute
    {
        public string SettingName { get; }

        public SaveToConfigAttribute(string settingName)
        {
            SettingName = settingName;
        }
    }
}