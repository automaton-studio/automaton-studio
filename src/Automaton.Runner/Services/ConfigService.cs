using Automaton.Runner.Core.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Automaton.Runner.Core.Services
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class ConfigService
    {
        #region Constants

        private const string CompanyName = "Automaton";
        private const string ApplicationName = "AutomatonStudio";
        private const string UserSettingsFileName = "UserConfig.json";
        private const string ApiConfigurationName = "ApiConfiguration";

        #endregion

        #region Members

        private readonly IConfiguration configuration;

        #endregion

        #region Properties

        public UserConfig UserConfig { get; private set; } = new UserConfig();
        public ApiConfig ApiConfig { get; private set; } = new ApiConfig();

        #endregion

        public ConfigService(IConfiguration configuration)
        {
            this.configuration = configuration;

            LoadStudioConfig();
            LoadUserConfig();
        }

        #region Public Methods

        public void RegisterRunner(string runnerName)
        {
            UserConfig.RunnerName = runnerName;
            SaveUserConfig();
        }

        #endregion

        #region Private Methods

        private void LoadStudioConfig()
        {
            configuration.GetSection(ApiConfigurationName).Bind(ApiConfig);
        }

        private void CreateUserConfig()
        {
            SaveUserConfig();
        }

        private void SaveUserConfig()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var folderPath = Path.Combine(appData, CompanyName, ApplicationName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, UserSettingsFileName);
            var userConfigJson = JsonConvert.SerializeObject(UserConfig, Formatting.Indented);

            File.WriteAllText(filePath, userConfigJson);
        }

        private void LoadUserConfig()
        {
            if (!UserConfigExists())
            {
                CreateUserConfig();
            }

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appData, CompanyName, ApplicationName, UserSettingsFileName);
           
            var userConfigJson = File.ReadAllText(filePath);
            UserConfig = JsonConvert.DeserializeObject<UserConfig>(userConfigJson);
        }

        private static bool UserConfigExists()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appData, CompanyName, ApplicationName, UserSettingsFileName);

            return File.Exists(filePath);
        }

        #endregion
    }
}
