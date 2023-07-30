using Automaton.Runner.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

public class SetupViewModel
{
    private readonly RunnerService runnerService;

    public string RunnerName { get; set; }

    public string ServerUrl { get; set; }

    public SetupViewModel(RunnerService registrationService)
    {
        this.runnerService = registrationService;
    }

    public async Task RegisterRunner()
    {
        await runnerService.Register(RunnerName, ServerUrl);
    }
}
