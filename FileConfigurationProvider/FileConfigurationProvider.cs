using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Task2_Plugins;

namespace FileConfigurationProvider
{
    public class FileConfigurationProvider : IConfigurationProvider
    {
        private static string _appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        private static string _json = File.ReadAllText(_appSettingsPath);
        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();
        private static dynamic _config;
        private static IDictionary<string, object> _expando;

        public FileConfigurationProvider()
        {
            _jsonSettings.Converters.Add(new ExpandoObjectConverter());
            _jsonSettings.Converters.Add(new StringEnumConverter());
            _config = JsonConvert.DeserializeObject<ExpandoObject>(_json, _jsonSettings);

            _expando = _config as IDictionary<string, object>;
        }

        public string ReadSetting(string key)
        {
            var result = "";
            try
            {
                result = _expando.TryGetValue(key, out var value) ? value?.ToString() : "";
            }
            catch (Exception)
            {
                Console.WriteLine("Error reading app settings");
            }

            return result;
        }

        public void SaveSetting(string key, string value)
        {
            try
            {
                if (!_expando.ContainsKey(key))
                {
                    _expando.Add(key, value);
                }
                else
                {
                    _expando[key] = value;
                }

                var newJson = JsonConvert.SerializeObject(_config, Formatting.Indented, _jsonSettings);
                File.WriteAllText(_appSettingsPath, newJson);
            }
            catch (Exception)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}