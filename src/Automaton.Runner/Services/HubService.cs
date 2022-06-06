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
    private readonly IAuthenticationStorage storageService;
    private readonly ILogger<HubService> logger;

    public HubService(ConfigService configService,
        FlowService workflowService,
        AuthStateProvider authStateProvider,
        IAuthenticationStorage storageService,
        ILogger<HubService> logger)
    {
        this.configService = configService;
        this.workflowService = workflowService;
        this.storageService = storageService;
        this.authStateProvider = authStateProvider;
        this.logger = logger;
    }

    public async Task<bool> Connect(string runnerName)
    {
        try
        {
            var studioConfig = configService.ApiConfig;
            var token = await storageService.GetAuthToken();
            var hubUrl = $"{studioConfig.BaseUrl}{studioConfig.WorkflowHubUrl}";

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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }

        return true;
    }

    public async Task Disconnect()
    {
        await connection.StopAsync();
        await connection.DisposeAsync();
    }

    public async Task Ping(string runnerName)
    {
        var result = await connection.InvokeAsync<bool>(PingMethod, runnerName);
    }
}
