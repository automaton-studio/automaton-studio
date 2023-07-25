using Automaton.App.Authentication.Config;
using Automaton.Runner.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigService
{
    private readonly IConfiguration configuration;
    private readonly ApplicationStorage applicationStorage;

    private readonly AppConfig appConfig = new();
    private readonly ApiConfig apiConfig = new();
    private readonly AuthenticationConfig authenticationConfig = new();

    public string RunnerName => appConfig.RunnerName;
    public string BaseUrl => apiConfig.BaseUrl;
    public string WorkflowHubUrl => apiConfig.WorkflowHubUrl;
    public string FlowsUrl => apiConfig.FlowsUrl;
    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        applicationStorage = new ApplicationStorage();

        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfig);
        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);
        appConfig = applicationStorage.GetApplicationConfiguration();
    }
}
