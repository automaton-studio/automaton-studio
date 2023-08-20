using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Dashboard;

public class DashboardViewModel
{
    private ConfigurationService configService;
    private HubService hubService;

    public string ConnectionText { get; set; }
    public string ConnectionIcon { get; set; }
    public string RunnerId { get; set; }
    public string RunnerName { get; set; }

    public DashboardViewModel(HubService hubService, ConfigurationService configService)
    {
        this.hubService = hubService;
        this.configService = configService;
    }

    public async Task ConnectHub()
    {
        RunnerId = configService.IsRunnerRegistered() ? configService.RunnerId : Messages.RunnerNotRegistered;
        RunnerName = configService.IsRunnerRegistered() ? configService.RunnerName : Messages.RunnerNotRegistered;

        await hubService.ConnectToServer();
    }

    public void SetHubConnection(HubConnectionState hubConnectionState)
    {
        switch (hubConnectionState)
        {
            case HubConnectionState.Connecting:
            case HubConnectionState.Reconnecting:
                SetConnecting();
                break;
            case HubConnectionState.Connected:
                SetConnected();
                break;
            case HubConnectionState.Disconnected:
                SetDisconnected();
                break;
        }
    }

    public bool IsRunnerConnected()
    {
        return hubService.IsConnected();
    }

    private void SetConnecting()
    {
        ConnectionText = Messages.Connecting;
        ConnectionIcon = "status-connecting";
    }

    private void SetConnected()
    {
        ConnectionText = Messages.Connected;
        ConnectionIcon = "status-connected";
    }

    private void SetDisconnected()
    {
        ConnectionText = Messages.Disconnected;
        ConnectionIcon = "status-disconnected";
    }
}

