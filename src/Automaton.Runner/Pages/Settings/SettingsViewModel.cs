using Automaton.Runner.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Settings;

public class SettingsViewModel
{
    private readonly RunnerService runnerService;
    private readonly ConfigService configService;

    public string RunnerName { get; set; }
    public string ServerUrl { get; set; }

    public SettingsViewModel(RunnerService runnerService, ConfigService configService)
    {
        this.configService = configService;
        this.runnerService = runnerService;
    }

    public void LoadSettings()
    {
        RunnerName = configService.RunnerName;
        ServerUrl = configService.ServerUrl;
    }

    public async Task SaveSettings()
    {
        await runnerService.UpdateRunnerDetails(RunnerName, ServerUrl);
    }
}
