using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration configuration;
        private readonly AppConfiguration appConfiguration = new();
        private readonly ApiConfiguration apiConfiguration = new();

        public string BaseUrl => apiConfiguration.BaseUrl;
        public string FlowsUrl => apiConfiguration.FlowsUrl;
        public string RunnersUrl => apiConfiguration.RunnersUrl;
        public bool IsDesktop => appConfiguration.IsDesktop;

        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.GetSection(nameof(AppConfiguration)).Bind(appConfiguration);
            this.configuration.GetSection(nameof(ApiConfiguration)).Bind(apiConfiguration);
        }
    }
}
