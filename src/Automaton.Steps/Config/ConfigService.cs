using Microsoft.Extensions.Configuration;

namespace Automaton.Steps.Config;

public class ConfigService
{
    private readonly IConfiguration configuration;

    public ApiConfig ApiConfig { get; private set; }

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        ApiConfig = new ApiConfig();
        
        LoadApiConfig();
    }

    private void LoadApiConfig()
    {
        configuration.GetSection(nameof(Config.ApiConfig)).Bind(ApiConfig);
    }
}
