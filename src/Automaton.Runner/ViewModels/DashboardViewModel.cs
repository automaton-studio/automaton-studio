using Automaton.Runner.Core.Services;
using System;
using System.Net.Http;
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

    public async Task<bool> ConnectToHub()
    {
        return await hubService.Connect(configService.AppConfig.RunnerName);
    }
}
