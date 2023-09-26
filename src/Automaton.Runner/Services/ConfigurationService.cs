using Automaton.Client.Auth.Models;
using Automaton.Runner.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;

    private readonly AppConfig appConfig = new();
    private readonly ClientAuthConfig authenticationConfig = new();
    private readonly ApplicationStorage applicationStorage = new();

    public string RunnerId => applicationStorage.GetRunnerId();
    public string ApplicationName => applicationStorage.GetRunnerName();
    public string ApplicationType => appConfig.ApplicationType;

    public string BaseUrl => appConfig.BaseUrl;
    public string WorkflowHubUrl => appConfig.WorkflowHubUrl;
    public string FlowsUrl => appConfig.FlowsUrl;
    public string RunnersUrl => appConfig.RunnersUrl;
    public string LogsUrl => appConfig.LogsUrl;
    public string FlowExecutionUrl => appConfig.FlowExecutionUrl;

    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(AppConfig)).Bind(appConfig);
        this.configuration.GetSection(nameof(ClientAuthConfig)).Bind(authenticationConfig);
    }

    public bool IsRunnerRegistered() => applicationStorage.IsRunnerRegistered();
}
