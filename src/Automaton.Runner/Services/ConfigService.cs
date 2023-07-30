using Automaton.App.Authentication.Config;
using Automaton.Runner.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigService
{
    private readonly IConfiguration configuration;

    private readonly AppConfig appConfig = new();
    private readonly ApiConfig apiConfig = new();
    private readonly AuthenticationConfig authenticationConfig = new();

    public string RunnerName => appConfig.RunnerName;
    public string ServerUrl => appConfig.ServerUrl;
    public bool RunnerRegistered => appConfig.RunnerRegistered;
    public string BaseUrl => apiConfig.BaseUrl;
    public string WorkflowHubUrl => apiConfig.WorkflowHubUrl;
    public string FlowsUrl => apiConfig.FlowsUrl;
    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfig);
        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);

        var applicationService = new ApplicationService();
        appConfig = applicationService.GetApplicationConfiguration();
    }
}
