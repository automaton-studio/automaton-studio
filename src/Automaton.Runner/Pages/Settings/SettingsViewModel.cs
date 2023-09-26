using Automaton.Runner.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Settings;

public class SettingsViewModel
{
    private readonly RunnerService runnerService;
    private readonly ConfigurationService configService;

    public string RunnerName { get; set; }

    public SettingsViewModel(RunnerService runnerService, ConfigurationService configService)
    {
        this.configService = configService;
        this.runnerService = runnerService;
    }

    public void LoadSettings()
    {
        RunnerName = configService.ApplicationName;
    }

    public async Task SaveSettings()
    {
        await runnerService.UpdateRunner(RunnerName);
    }
}
