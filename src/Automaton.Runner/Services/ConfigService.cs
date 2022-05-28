using Automaton.Runner.Core.Config;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;

namespace Automaton.Runner.Core.Services;

public class ConfigService
{
    #region Constants

    private const string ApiConfigurationName = "ApiConfiguration";

    #endregion

    #region Members

    private readonly IConfiguration configuration;
    private readonly ApplicationStorage applicationStorage = new ApplicationStorage();
    #endregion

    #region Properties

    public AppConfig AppConfig { get; private set; } = new AppConfig();
    public ApiConfig ApiConfig { get; private set; } = new ApiConfig();

    #endregion

    public ConfigService(IConfiguration configuration)
    {
        this.configuration = configuration;

        LoadStudioConfig();
        LoadAppConfig();
    }

    #region Private Methods

    private void LoadStudioConfig()
    {
        configuration.GetSection(ApiConfigurationName).Bind(ApiConfig);
    }

    private void LoadAppConfig()
    {
        AppConfig = applicationStorage.GetApplicationConfiguration();
    }

    #endregion
}
