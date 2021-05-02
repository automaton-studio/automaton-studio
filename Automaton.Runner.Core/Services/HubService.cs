using Automaton.Runner.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Automaton.Runner.Core.Services
{
    public class HubService : IHubService
    {
        #region Constants

        private const string RunnerNameHeader = "RunnerName";
        private const string RunWorkflowMethod = "RunWorkflow";
        private const string WelcomeRunnerMethod = "WelcomeRunner";
        private const string PingMethod = "Ping";

        #endregion

        #region Private Members

        private HubConnection connection;
        private readonly IWorkflowService workflowService;
        private readonly ConfigService configService;

        #endregion

        #region Constructors

        public HubService(ConfigService configService, IWorkflowService workflowService)
        {
            this.configService = configService;
            this.workflowService = workflowService;
        }

        #endregion

        #region Public Methods

        public async Task Connect(JsonWebToken token, string runnerName)
        {
            var studioConfig = configService.StudioConfig;

            connection = new HubConnectionBuilder()
                .WithUrl(studioConfig.WorkflowHubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token.AccessToken);
                    options.Headers.Add(RunnerNameHeader, runnerName);
                })
                .Build();

            connection.On<string>(RunWorkflowMethod, (definitionId) =>
            {
                this.workflowService.RunWorkflow(definitionId);
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
