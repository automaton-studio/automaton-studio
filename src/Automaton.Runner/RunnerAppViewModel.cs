using Automaton.Runner.Services;

namespace Automaton.Runner;

public class RunnerAppViewModel
{
    private ConfigurationService configService;

    public RunnerAppViewModel(ConfigurationService configService)
    {
        this.configService = configService;
    }
}
