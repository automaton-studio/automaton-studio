using Automaton.Runner.Services;

namespace Automaton.Runner;

public class RunnerAppViewModel
{
    private ConfigService configService;

    public RunnerAppViewModel(ConfigService configService)
    {
        this.configService = configService;
    }
}
