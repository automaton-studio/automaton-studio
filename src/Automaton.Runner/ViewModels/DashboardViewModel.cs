using Automaton.Runner.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class DashboardViewModel
{
    private readonly HubService hubService;
    private readonly ConfigService configService;
    private readonly ILogger<DashboardViewModel> logger;
    private readonly MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

    public DashboardViewModel(HubService hubService, ConfigService configService, ILogger<DashboardViewModel> logger)
    {
        this.hubService = hubService;
        this.configService = configService;
        this.logger = logger;

        hubService.Connected += HubServiceConnected;
        hubService.Disconnected += HubServiceDisconnected;
    }

    private void HubServiceConnected(object sender, EventArgs e)
    {
        mainWindow.ConnectedToServer();
    }

    private void HubServiceDisconnected(object sender, EventArgs e)
    {
        mainWindow.DisconnectedFromServer();
    }

    public async Task ConnectToServer()
    {      
        await hubService.ConnectToServer(configService.AppConfig.RunnerName);     
    }

}
