using Automaton.Runner.Core.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services
{
    public class ConfigService
    {
        #region Members

        private readonly IConfiguration configuration;

        #endregion

        #region Properties

        public AppConfiguration AppConfig { get; private set; } = new AppConfiguration();
        public string WebApiUrl => AppConfig.WebApiUrl;
        public bool IsDesktop => AppConfig.IsDesktop;
        public string FlowsUrl =>$"{AppConfig.WebApiUrl}/api/flows";
        public string TokenUrl => $"/api/token";

        #endregion

        public ConfigService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.GetSection(nameof(AppConfiguration)).Bind(AppConfig);
        }
    }
}
