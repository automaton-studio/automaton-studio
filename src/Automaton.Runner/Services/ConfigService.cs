using Automaton.App.Authentication.Config;
using Automaton.Runner.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigService
{
    private readonly IConfiguration configuration;

    private readonly ApiConfig apiConfig = new();
    private readonly AuthenticationConfig authenticationConfig = new();
    private readonly ApplicationStorage applicationStorage = new();

    public string ServerUrl => applicationStorage.GetServerUrl();
    public string RunnerId => applicationStorage.GetRunnerId();
    public string RunnerName => applicationStorage.GetRunnerName();
    public string BaseUrl => apiConfig.BaseUrl;
    public string WorkflowHubUrl => apiConfig.WorkflowHubUrl;
    public string FlowsUrl => apiConfig.FlowsUrl;
    public string RegistrationUrl => apiConfig.RegistrationUrl;
    public string RunnerByNameUrl => apiConfig.RunnerByNameUrl;
    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfig);
        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);
    }

    public bool IsRunnerRegistered() => applicationStorage.IsRunnerRegistered();
    public bool IsServerRegistered() => applicationStorage.IsServerRegistered();
}
