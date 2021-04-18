using Automaton.Runner.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Core.Services
{
    public class HubService : IHubService
    {
        #region Constants

        private const string RunnerNameHeader = "RunnerName";

        #endregion

        #region Private Members

        private HubConnection connection;
        private readonly IWorkflowService workflowService;
        private readonly IAppConfigurationService configService;

        #endregion

        #region Constructors

        public HubService(IAppConfigurationService configService,
                       IWorkflowService workflowService)
        {
            this.configService = configService;
            this.workflowService = workflowService;
        }

        #endregion

        #region Public Methods

        public async Task Connect(JsonWebToken token, string runnerName)
        {
            var studioConfig = configService.GetStudioConfig();

            connection = new HubConnectionBuilder()
                    .WithUrl(studioConfig.WorkflowHubUrl, options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token.AccessToken);
                        options.Headers.Add(RunnerNameHeader, runnerName);
                    })
                    .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string>("RunWorkflow", (definitionId) =>
            {
                this.workflowService.RunWorkflow(definitionId);
            });

            connection.On<string>("WelcomeRunner", (name) =>
            {
            });

            await connection.StartAsync();
        }

        public async Task Register(string runnerName)
        {
            await connection.SendAsync("RegisterRunner", runnerName);
        }

        #endregion
        }
    }
