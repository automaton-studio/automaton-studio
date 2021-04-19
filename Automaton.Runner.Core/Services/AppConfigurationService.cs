using Automaton.Runner.Core.Config;
using Automaton.Runner.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;

namespace Automaton.Runner.Core.Services
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class AppConfigurationService : IAppConfigurationService
    {
        private const string CompanyName = "Automaton";
        private const string ApplicationName = "AutomatonStudio";
        private const string UserSettings = "UserConfig.json";

        private readonly IConfiguration configuration;

        public AppConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Retrieve Studio configuration
        /// </summary>
        /// <returns>Studio configuration</returns>
        public StudioConfig GetStudioConfig()
        { 
            var config = new StudioConfig();
            configuration.GetSection(nameof(StudioConfig)).Bind(config);

            return config;
        }

        public UserConfig GetUserConfig()
        {
            var userConfig = new UserConfig();
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appData, CompanyName, ApplicationName, UserSettings);
            
            if (File.Exists(filePath))
            {
                var userConfigJson = File.ReadAllText(filePath);
                userConfig = JsonConvert.DeserializeObject<UserConfig>(userConfigJson);
            }

            return userConfig;
        }

        public void SetUserConfig(UserConfig userConfig)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var folderPath = Path.Combine(appData, CompanyName, ApplicationName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, UserSettings);
            var userConfigJson = JsonConvert.SerializeObject(userConfig, Formatting.Indented);

            File.WriteAllText(filePath, userConfigJson);
        }
    }
}
