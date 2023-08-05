using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Dashboard;

public class DashboardViewModel
{
    private ConfigService configService;
    private HubService hubService;

    public string ConnectionText { get; set; }
    public string ConnectionIcon { get; set; }
    public string ServerUrl { get; set; }
    public string RunnerName { get; set; }

    public DashboardViewModel(HubService hubService, ConfigService configService)
    {
        this.hubService = hubService;
        this.configService = configService;

        hubService.Connected += HubServiceConnected;
        hubService.Disconnected += HubServiceDisconnected;
    }

    public async Task ConnectHub()
    {
        RunnerName = configService.IsRunnerRegistered() ? configService.RunnerName : Messages.RunnerNotRegistered;
        ServerUrl = configService.IsServerRegistered() ? configService.ServerUrl : Messages.ServerNotRegistered;

        if (configService.IsRunnerRegistered())
        {
            SetConnecting();

            await hubService.ConnectToServer(configService.RunnerName);
        }
    }

    public bool IsRunnerConnected()
    {
        return hubService.IsConnected();
    }

    private void HubServiceConnected(object sender, EventArgs e)
    {
        SetConnected();
    }

    private void HubServiceDisconnected(object sender, EventArgs e)
    {
        SetDisconnected();
    }

    public void SetConnecting()
    {
        ConnectionText = Messages.Connecting;
        ConnectionIcon = "status-connecting";
    }

    public void SetConnected()
    {
        ConnectionText = Messages.Connected;
        ConnectionIcon = "status-connected";
    }

    public void SetDisconnected()
    {
        ConnectionText = Messages.Disconnected;
        ConnectionIcon = "status-disconnected";
    }

}

