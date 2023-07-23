using Automaton.Runner.Core.Config;
using Newtonsoft.Json;
using System.Windows;

namespace Automaton.Runner.Storage;

public class ApplicationStorage
{
    private const string ApplicationConfiguration = "applicationConfiguration";
    private readonly App application = (App)Application.Current;

    public AppConfig GetApplicationConfiguration()
    {
        if(application.Properties.Contains(ApplicationConfiguration))
        {
            var applicationConfigurationProperty = application.Properties[ApplicationConfiguration];
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(applicationConfigurationProperty.ToString());
            return appConfig;
        }

        return new AppConfig();
    }

    public void SetRunnerName(string runnerName)
    {
        var appConfig = GetApplicationConfiguration();

        appConfig.RunnerName = runnerName;

        SetApplicationConfiguration(appConfig);
    }

    private void SetApplicationConfiguration(AppConfig applicationConfiguration)
    {
        application.Properties[ApplicationConfiguration] = applicationConfiguration;
    }
}
