using GofileDownloader.Models;
using GofileDownloader.Utils;
using Spectre.Console;
using System.Runtime.Serialization;
using System.Text.Json;

namespace GofileDownloader.Helpers
{
    public class ConfigHelper
    {
        private static readonly JsonSerializerOptions jsonSerOptions = new() { WriteIndented = true};
        public static Config LoadConfig()
        {
            if (!IsConfigFileExists())
            {
                GenerateConfigFile();
            }

            string configText = File.ReadAllText(Constants.Paths.CONFIG_PATH);

            if (!string.IsNullOrEmpty(configText))
            {
                try
                {
                    return JsonSerializer.Deserialize<Config>(configText);
                }
                catch (SerializationException ex)
                {
                    throw ex;
                }

            }

            throw new Exception("Config file is empty.");
        }
        
        public static void SaveConfig(Config config)
        {
            if (IsConfigFileExists())
            {
                string json = JsonSerializer.Serialize(config, jsonSerOptions);
                File.WriteAllText(Constants.Paths.CONFIG_PATH, json);
            }
            else throw new FileNotFoundException();
        }

        private static bool IsConfigFileExists()
        {
            return (File.Exists(Constants.Paths.CONFIG_PATH));
        }

        public static void GenerateConfigFile()
        {
            Config config = new() { Token = ConsoleHelper.PromptUserForToken() };
            AnsiConsole.Clear();

            string json = JsonSerializer.Serialize(config, jsonSerOptions);
            File.WriteAllText(Constants.Paths.CONFIG_PATH, json);
        }
    }
}
