using Automaton.Runner.Core.Config;

namespace Automaton.Runner.Services
{
    public interface IAppConfigurationService
    {
        StudioConfig GetStudioConfig();

        UserConfig GetUserConfig();
        void SetUserConfig(UserConfig userConfig);
    }
}
