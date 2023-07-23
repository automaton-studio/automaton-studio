using Automaton.Runner.Core.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Services;

public class ConfigService
{
    private readonly IConfiguration configuration;
    private readonly ApplicationStorage applicationStorage;

    public AppConfig AppConfig { get; private set; }
    public ApiConfig ApiConfig { get; private set; }

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        applicationStorage = new ApplicationStorage();
        AppConfig = new AppConfig();
        ApiConfig = new ApiConfig();
        
        LoadApiConfig();
        LoadAppConfig();
    }

    private void LoadApiConfig()
    {
        configuration.GetSection(nameof(ApiConfig)).Bind(ApiConfig);
    }

    private void LoadAppConfig()
    {
        AppConfig = applicationStorage.GetApplicationConfiguration();
    }
}
