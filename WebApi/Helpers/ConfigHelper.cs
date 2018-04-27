using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Helpers
{
    public class ConfigHelper
    {
        public static string ContentPath { get; set; }
        public const string POLITICIANS_FOLDER = "politicians";
        public const string TRACKS_FOLDER = "tracks";
        public static Config GetConfig()
        {
            Config config = null;
            var configPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{ConfigurationManager.AppSettings["ConfigFilePath"]}";            
            if (File.Exists(configPath))
            {
                string configString = File.ReadAllText(configPath);
                config = JsonConvert.DeserializeObject<Config>(configString);
            }

            return config;
        }

        public static void SaveConfig(Config config)
        {
            var configPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{ConfigurationManager.AppSettings["ConfigFilePath"]}";
            if (File.Exists(configPath))
            {
                string configString = JsonConvert.SerializeObject(config);
                File.WriteAllText(configPath, configString);
            }
        }
    }
}