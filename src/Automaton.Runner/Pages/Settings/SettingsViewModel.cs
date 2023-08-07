using Automaton.Runner.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Settings;

public class SettingsViewModel
{
    private readonly RunnerService runnerService;
    private readonly ConfigService configService;

    public string RunnerName { get; set; }

    public SettingsViewModel(RunnerService runnerService, ConfigService configService)
    {
        this.configService = configService;
        this.runnerService = runnerService;
    }

    public void LoadSettings()
    {
        RunnerName = configService.RunnerName;
    }

    public async Task SaveSettings()
    {
        await runnerService.UpdateRunnerDetails(RunnerName);
    }
}
