using Automaton.App.Authentication.Config;
using Automaton.Client.Auth.Models;
using Automaton.Runner.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigService
{
    private readonly IConfiguration configuration;

    private readonly Automaton.Runner.Config.RunnerConfig apiConfig = new();
    private readonly ClientAuthConfig authenticationConfig = new();
    private readonly ApplicationStorage applicationStorage = new();

    public string RunnerId => applicationStorage.GetRunnerId();
    public string RunnerName => applicationStorage.GetRunnerName();

    public string BaseUrl => apiConfig.BaseUrl;
    public string WorkflowHubUrl => apiConfig.WorkflowHubUrl;
    public string FlowsUrl => apiConfig.FlowsUrl;
    public string RunnersUrl => apiConfig.RunnersUrl;

    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(Automaton.Runner.Config.RunnerConfig)).Bind(apiConfig);
        this.configuration.GetSection(nameof(ClientAuthConfig)).Bind(authenticationConfig);
    }

    public bool IsRunnerRegistered() => applicationStorage.IsRunnerRegistered();
}
