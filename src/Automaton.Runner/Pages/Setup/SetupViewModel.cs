using Automaton.Runner.Services;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

public class SetupViewModel
{
    private readonly RunnerService runnerService;

    public string RunnerName { get; set; }
    public int CurrentSetupStep { get; set; }
    public Type SetupStepComponent { get; set; }
    public Dictionary<string, object> SetupStepParameters { get; set; }

    public SetupViewModel(RunnerService registrationService)
    {
        this.runnerService = registrationService;

        SetupStepComponent = typeof(SetupDetails);

        SetupStepParameters = new Dictionary<string, object>()
        {
            { "SetupViewModel", this }
        };
    }

    public async Task SetupRunner()
    {
        await runnerService.SetupRunnerDetails(RunnerName);
    }

    public void NavigateToDetails()
    {
        SetupStepComponent = typeof(SetupDetails);
        CurrentSetupStep = 0;
    }

    public void NavigateToInstallation()
    {
        SetupStepComponent = typeof(SetupDetails);
        CurrentSetupStep = 1;
    }
}
