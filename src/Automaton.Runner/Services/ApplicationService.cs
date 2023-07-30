using Automaton.Runner.Config;
using Newtonsoft.Json;
using System.Windows;

namespace Automaton.Runner.Storage;

public class ApplicationService
{
    private const string ApplicationConfig = "ApplicationConfig";
    private readonly App application = (App)Application.Current;

    public AppConfig GetApplicationConfiguration()
    {
        var appConfig = AppConfigExists() ? GetAppConfig() : NewAppConfig();

        return appConfig;
    }

    public void SetRunnerName(string runnerName)
    {
        var appConfig = GetApplicationConfiguration();

        appConfig.RunnerName = runnerName;

        SaveAppConfig(appConfig);
    }

    public void SetServerUrl(string serverUrl)
    {
        var appConfig = GetApplicationConfiguration();

        appConfig.ServerUrl = serverUrl;

        SaveAppConfig(appConfig);
    }

    private void SaveAppConfig(AppConfig applicationConfiguration)
    {
        application.Properties[ApplicationConfig] = applicationConfiguration;
    }

    private AppConfig? GetAppConfig()
    {
        var applicationConfigProperty = application.Properties[ApplicationConfig];
        var appConfig = JsonConvert.DeserializeObject<AppConfig>(applicationConfigProperty.ToString());

        return appConfig;
    }

    private bool AppConfigExists()
    {
        return application.Properties.Contains(ApplicationConfig);
    }

    private AppConfig NewAppConfig()
    {
        return new AppConfig();
    }
}
