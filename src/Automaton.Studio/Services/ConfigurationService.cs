using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AppConfig appConfig = new();
    private readonly ApiConfig apiConfig = new();

    public string BaseUrl => apiConfig.BaseUrl;
    public string FlowsUrl => apiConfig.FlowsUrl;
    public string CustomStepsUrl => apiConfig.CustomStepsUrl;
    public string RunnersUrl => apiConfig.RunnersUrl;
    public string LogsUrl => apiConfig.LogsUrl;
    public bool IsDesktop => appConfig.IsDesktop;
    public string ApplicationName => appConfig.ApplicationName;
    public string ApplicationType => appConfig.ApplicationType;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AppConfig)).Bind(appConfig);
        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfig);
    }
}
