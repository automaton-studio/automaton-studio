using Automaton.Runner.Core.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class DashboardViewModel
{
    private readonly HubService hubService;
    private readonly ConfigService configService;

    public DashboardViewModel(HubService hubService, ConfigService configService)
    {
        this.hubService = hubService;
        this.configService = configService;
    }

    public async Task ConnectToHub()
    {
        await hubService.Connect(configService.AppConfig.RunnerName);
    }
}
