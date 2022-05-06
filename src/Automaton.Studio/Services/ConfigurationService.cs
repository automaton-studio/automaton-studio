using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services
{
    public class ConfigurationService
    {
        #region Members

        private readonly IConfiguration configuration;
        private readonly AppConfiguration appConfiguration = new();

        #endregion

        #region Properties

        public string WebApiUrl => appConfiguration.WebApiUrl;
        public bool IsDesktop => appConfiguration.IsDesktop;
        public string FlowsUrl => appConfiguration.FlowsUrl;
        
        #endregion

        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.GetSection(nameof(AppConfiguration)).Bind(appConfiguration);
        }
    }
}
