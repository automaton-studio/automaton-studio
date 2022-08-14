using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Automaton.Runner.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Core.Services;

public class HubService
{
    private const string RunnerNameHeader = "RunnerName";
    private const string RunWorkflow = "RunWorkflow";
    private const string WelcomeRunner = "WelcomeRunner";
    private const string PingMethod = "Ping";

    private HubConnection connection;
    private readonly FlowService workflowService;
    private readonly AuthStateProvider authStateProvider;
    private readonly ConfigService configService;
    private readonly ILogger<HubService> logger;

    public HubService(ConfigService configService,
        FlowService workflowService,
        AuthStateProvider authStateProvider,
        IAuthenticationStorage storageService)
    {
        this.configService = configService;
        this.workflowService = workflowService;
        this.authStateProvider = authStateProvider;
    }

    public async Task Connect(string runnerName)
    {
        var apiConfig = configService.ApiConfig;
        var hubUrl = $"{apiConfig.BaseUrl}{apiConfig.WorkflowHubUrl}";

        connection = new HubConnectionBuilder().WithUrl(hubUrl, options =>
        {
            options.AccessTokenProvider = async () => await authStateProvider.GetAccessTokenAsync();
            options.Headers.Add(RunnerNameHeader, runnerName);
        })
        .Build();

        connection.On<Guid>(RunWorkflow, async (workflowId) =>
        {
            await workflowService.RunFlow(workflowId);
        });

        connection.On<string>(WelcomeRunner, (name) =>
        {
        });

        await connection.StartAsync();
    }

    public async Task Disconnect()
    {
        if (connection?.State == HubConnectionState.Connected)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
        }
    }

    public async Task Ping(string runnerName)
    {
        var result = await connection.InvokeAsync<bool>(PingMethod, runnerName);
    }
}
