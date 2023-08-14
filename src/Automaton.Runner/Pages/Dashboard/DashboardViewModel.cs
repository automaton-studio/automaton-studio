using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using System;
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

        hubService.Connected += HubServiceConnected;
        hubService.Disconnected += HubServiceDisconnected;
    }

    public async Task ConnectHub()
    {
        try
        {
            RunnerId = configService.IsRunnerRegistered() ? configService.RunnerId : Messages.RunnerNotRegistered;
            RunnerName = configService.IsRunnerRegistered() ? configService.RunnerName : Messages.RunnerNotRegistered;

            if (configService.IsRunnerRegistered())
            {
                SetConnecting();

                await hubService.ConnectToServer();
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public bool IsRunnerConnected()
    {
        return hubService.IsConnected();
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

    private void HubServiceConnected(object sender, EventArgs e)
    {
        SetConnected();
    }

    private void HubServiceDisconnected(object sender, EventArgs e)
    {
        SetDisconnected();
    }
}

