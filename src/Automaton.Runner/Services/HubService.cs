using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Automaton.Core.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
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
    }

    public async Task ConnectToServer()
    {
        try
        {
            BuildConnection();

            await Connect();
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
            .WithAutomaticReconnect()
            .Build();

        connection.On<Guid>(RunWorkflowMethod, RunWorkflow);
        connection.On<string>(PingMethod, Ping);

        connection.Closed += ConnectionClosed;
        connection.Reconnecting += ConnectionReconnecting;
        connection.Reconnected += ConnectionReconnected;
    }

    private async Task Connect()
    {
        logger.Information("Runner {0} is connecting to server {1}", configService.RunnerName, hubServer);

        await connection.StartAsync();

        logger.Information("Runner {0} connected to server {1}", configService.RunnerName, hubServer);

        await mediator.Publish(new HubConnectionNotification(HubConnectionState.Connected));
    }

    private async Task ConnectionClosed(Exception? arg)
    {
        try
        {
            logger.Information("Runner {0} disconnected from server {1}", configService.RunnerName, hubServer);

            await mediator.Publish(new HubConnectionNotification(HubConnectionState.Disconnected));
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occured when runner {0} was reconnecting to server {1}", configService.RunnerName, hubServer);
        }
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

    private async Task RunWorkflow(Guid workflowId)
    {
        await workflowService.RunFlow(workflowId);
    }

    private async Task<string> Ping(string name)
    {
        return await Task.Run(() => "Pong");
    }
}
