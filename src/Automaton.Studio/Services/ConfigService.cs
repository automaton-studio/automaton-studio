using Automaton.Runner.Core.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class ConfigService
    {
        #region Members

        private readonly IConfiguration configuration;

        #endregion

        #region Properties

        public WebApiConfig WebApiConfig { get; private set; } = new WebApiConfig();
        public string WebApiUrl => WebApiConfig.WebApiUrl;
        public string FlowsUrl =>$"{WebApiConfig.WebApiUrl}/api/flows";

        #endregion

        public ConfigService(IConfiguration configuration)
        {
            this.configuration = configuration;

            LoadConductorConfig();
        }

        #region Private Methods

        private void LoadConductorConfig()
        {
            configuration.GetSection(nameof(WebApiConfig)).Bind(WebApiConfig);
        }

        #endregion
    }
}
