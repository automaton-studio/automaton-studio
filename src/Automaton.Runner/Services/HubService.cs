using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Automaton.Core.Events;
using Automaton.Runner.Connection;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Mono.Unix.Native;
using Polly;
using Polly.Timeout;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class HubService
{
    private const string RunnerIdHeader = "RunnerId";
    private const string RunnerNameHeader = "RunnerName";

    private const string RunWorkflowMethod = "RunWorkflow";
    private const string PingMethod = "Ping";

    private HubConnection connection;
    private readonly FlowService workflowService;
    private readonly AuthStateProvider authStateProvider;
    private readonly ConfigurationService configService;
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly string hubServer;

    private readonly IConnectionPolicy resilientConnectionPolicy;

    public HubService(ConfigurationService configService,
        FlowService workflowService,
        AuthStateProvider authStateProvider,
        IAuthenticationStorage storageService,
        IMediator mediator)
    {
        this.configService = configService;
        this.workflowService = workflowService;
        this.authStateProvider = authStateProvider;
        this.mediator = mediator;

        hubServer = $"{configService.BaseUrl}/{configService.WorkflowHubUrl}";
        logger = Log.ForContext<HubService>();
        this.mediator = mediator;

        resilientConnectionPolicy = new ResilientPolicy
        (
            retryAfterSeconds: 5, 
            exceptionsAllowedBeforeBreaking: 2, 
            durationOfBreak: TimeSpan.FromMinutes(1)
        );
    }

    public async Task ConnectToServer()
    {
        try
        {
            BuildConnection();

            await resilientConnectionPolicy.GetPolicy().ExecuteAsync(Connect);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Runner {0} could not connect to server {1}", configService.RunnerName, hubServer);
        }
    }

    public bool IsConnected()
    {
        return connection?.State == HubConnectionState.Connected;
    }

    private void BuildConnection()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(hubServer, options =>
            {
                options.AccessTokenProvider = async () => await authStateProvider.GetAccessTokenAsync();
                options.Headers.Add(RunnerIdHeader, configService.RunnerId);
                options.Headers.Add(RunnerNameHeader, configService.RunnerName);
            })
            .Build();

        connection.On<Guid>(RunWorkflowMethod, ExecuteWorkflow);
        connection.On<string>(PingMethod, Ping);

        connection.Closed += ConnectionClosed;
        connection.Reconnecting += ConnectionReconnecting;
        connection.Reconnected += ConnectionReconnected;
    }

    private async Task Connect()
    {
        try
        {
            logger.Information("Runner {0} is connecting to server {1}", configService.RunnerName, hubServer);

            await mediator.Publish(new HubConnectionNotification(HubConnectionState.Connecting));

            await connection.StartAsync();

            logger.Information("Runner {0} connected to server {1}", configService.RunnerName, hubServer);

            await mediator.Publish(new HubConnectionNotification(HubConnectionState.Connected));
        }
        catch (Exception ex)
        {
            await mediator.Publish(new HubConnectionNotification(HubConnectionState.Disconnected));

            logger.Error(ex, "An error happened when Runner {0} was connecting to server {1}", configService.RunnerName, hubServer);
            throw;
        }
    }

    private async Task ConnectionClosed(Exception? arg)
    {
        logger.Information("Runner {0} disconnected from server {1}", configService.RunnerName, hubServer);

        await mediator.Publish(new HubConnectionNotification(HubConnectionState.Disconnected));

        await resilientConnectionPolicy.GetPolicy().ExecuteAsync(Connect);
    }

    private async Task ConnectionReconnecting(Exception? arg)
    {
        await mediator.Publish(new HubConnectionNotification(HubConnectionState.Reconnecting));

        logger.Information("Runner {0} connecting to server {1}", configService.RunnerName, hubServer);
    }

    private async Task ConnectionReconnected(string? arg)
    {
        await mediator.Publish(new HubConnectionNotification(HubConnectionState.Connected));

        logger.Information("Runner {0} connected to server {1}", configService.RunnerName, hubServer);
    }

    private async Task ExecuteWorkflow(Guid workflowId)
    {
        await workflowService.ExecuteWorkflow(workflowId);
    }

    private async Task<string> Ping(string name)
    {
        return await Task.Run(() => "Pong");
    }
}
