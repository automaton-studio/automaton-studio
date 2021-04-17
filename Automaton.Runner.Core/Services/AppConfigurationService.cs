using Automaton.Runner.Core.Config;
using Automaton.Runner.Services;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Core.Services
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class AppConfigurationService : IAppConfigurationService
    {
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

        /// <summary>
        /// Retrieve general Application configuration
        /// </summary>
        /// <returns>Application configuration</returns>
        public AppConfig GetAppConfig()
        {
            var config = new AppConfig();
            configuration.GetSection(nameof(AppConfig)).Bind(config);

            return config;
        }

        public bool IsRunnerRegistered()
        {
            var config = GetAppConfig();
            var runnerRegistered = !string.IsNullOrEmpty(config.RunnerName);

            return runnerRegistered;
        }

        public string GetRunnerName()
        {
            var config = GetAppConfig();

            return config.RunnerName;
        }
    }
}
