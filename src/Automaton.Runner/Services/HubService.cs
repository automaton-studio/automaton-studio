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

        public HubService(ConfigService configService, WorkflowService workflowService, IStorageService storageService)
        {
            this.configService = configService;
            this.workflowService = workflowService;
            this.storageService = storageService;
        }

        public async Task Connect(string runnerName)
        {
            var studioConfig = configService.ApiConfig;
            var token = await storageService.GetAuthToken();
            var hubUrl = $"{studioConfig.WebApiUrl}{studioConfig.WorkflowHubUrl}";

            connection = new HubConnectionBuilder().WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
                options.Headers.Add(RunnerNameHeader, runnerName);
            })
            .Build();

            connection.On<Guid>(RunWorkflowMethod, async (workflowId) =>
            {
                await workflowService.RunWorkflow(workflowId);
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
    }
}
