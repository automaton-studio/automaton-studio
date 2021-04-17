using Automaton.Runner.Core.Config;

namespace Automaton.Runner.Services
{
    public interface IAppConfigurationService
    {
        AppConfig GetAppConfig();
        StudioConfig GetStudioConfig();

        bool IsRunnerRegistered();
        string GetRunnerName();
    }
}
