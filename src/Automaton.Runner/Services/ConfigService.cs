using Automaton.Runner.Core.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Core.Services;

public class ConfigService
{
    private const string ApiConfigurationName = "ApiConfiguration";

    private readonly IConfiguration configuration;
    private readonly ApplicationStorage applicationStorage = new ApplicationStorage();

    public AppConfig AppConfig { get; private set; } = new AppConfig();
    public ApiConfig ApiConfig { get; private set; } = new ApiConfig();

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        LoadApiConfig();
        LoadAppConfig();
    }

    private void LoadApiConfig()
    {
        configuration.GetSection(ApiConfigurationName).Bind(ApiConfig);
    }

    private void LoadAppConfig()
    {
        AppConfig = applicationStorage.GetApplicationConfiguration();
    }
}
