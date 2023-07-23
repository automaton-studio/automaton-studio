using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AppConfig appConfiguration = new();
    private readonly ApiConfig apiConfiguration = new();

    public string BaseUrl => apiConfiguration.BaseUrl;
    public string FlowsUrl => apiConfiguration.FlowsUrl;
    public string CustomStepsUrl => apiConfiguration.CustomStepsUrl;
    public string RunnersUrl => apiConfiguration.RunnersUrl;
    public string LogsUrl => apiConfiguration.LogsUrl;
    public bool IsDesktop => appConfiguration.IsDesktop;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AppConfig)).Bind(appConfiguration);
        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfiguration);
    }
}
