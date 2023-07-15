using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AppConfig appConfiguration = new();
    private readonly ApiConfig apiConfiguration = new();
    private readonly OptionalConfig optionalConfiguration = new();

    public string BaseUrl => apiConfiguration.BaseUrl;
    public string FlowsUrl => apiConfiguration.FlowsUrl;
    public string CustomStepsUrl => apiConfiguration.CustomStepsUrl;
    public string RunnersUrl => apiConfiguration.RunnersUrl;
    public string LogsUrl => apiConfiguration.LogsUrl;
    public int StepMarginOffset => appConfiguration.StepMarginOffset;
    public bool UserSignUp => optionalConfiguration.UserSignUp;
    public bool NoUserSignUp => !UserSignUp;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AppConfig)).Bind(appConfiguration);
        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfiguration);
        this.configuration.GetSection(nameof(OptionalConfig)).Bind(optionalConfiguration);
    }
}
