using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;

namespace Task2_Plugins
{
    public static class PluginLoader
    {
        public static Dictionary<string, IConfigurationProvider> ConfigurationProvidersPlugins =
            new Dictionary<string, IConfigurationProvider>();

        public static Dictionary<string, IConfigurationProvider> LoadPlugins(string path)
        {
            foreach (var pluginPath in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    var context = new AssemblyLoadContext(pluginPath);
                    var assembly = context.LoadFromAssemblyPath(pluginPath);
                    var provider = Activator.CreateInstance(assembly.GetTypes()[0]) as IConfigurationProvider;
                    ConfigurationProvidersPlugins.Add(Path.GetFileNameWithoutExtension(pluginPath), provider);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong:{e.Message}");
                }
            }

            if (ConfigurationProvidersPlugins.Count == 0)
                throw new ApplicationException("No Plugins were found... Please check plugins folder path.");

            return ConfigurationProvidersPlugins;
        }
    }
}