namespace Task2_Plugins
{
    public interface IConfigurationProvider
    {
        void SaveSetting(string key, string value);
        string ReadSetting(string key);
    }
}