using Automaton.Runner.Core.Services;
using Automaton.Runner.Resources;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

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
    }

    public async Task ConnectToHub()
    {
        try
        {
            await hubService.Connect(configService.AppConfig.RunnerName);
            mainWindow.ConnectedToServer();
        }
        catch (Exception ex)
        {
            mainWindow.DisconnectedFromServer();
            logger.LogError(ex, Errors.CannotConnectToServer);
        }
    }

}
