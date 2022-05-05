using Automaton.Client.Auth.Interfaces;
using Automaton.Runner.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Core.Services
{
    public class HubService
    {
        private const string RunnerNameHeader = "RunnerName";
        private const string RunWorkflowMethod = "RunWorkflow";
        private const string WelcomeRunnerMethod = "WelcomeRunner";
        private const string PingMethod = "Ping";

        private HubConnection connection;
        private readonly WorkflowService workflowService;
        private readonly ConfigService configService;
        private readonly IStorageService storageService;

        #region Constructors

        public HubService(ConfigService configService, WorkflowService workflowService, IStorageService storageService)
        {
            this.configService = configService;
            this.workflowService = workflowService;
            this.storageService = storageService;
        }

        #endregion

        #region Public Methods

        public async Task Connect(string runnerName)
        {
            var studioConfig = configService.StudioConfig;
            var token = await storageService.GetAuthToken();

            connection = new HubConnectionBuilder()
                .WithUrl(studioConfig.WorkflowHubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                    options.Headers.Add(RunnerNameHeader, runnerName);
                })
                .Build();

            connection.On<Guid>(RunWorkflowMethod, async (definitionId) =>
            {
                await workflowService.RunWorkflow(definitionId);
            });

            connection.On<string>(WelcomeRunnerMethod, (name) =>
            {
            });

            await connection.StartAsync();
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

        #endregion
    }
}
