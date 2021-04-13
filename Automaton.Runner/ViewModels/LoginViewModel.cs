using Automaton.Runner.Core;
using Automaton.Runner.Core.Auth;
using Automaton.Runner.Core.Config;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private HubConnection connection;
        private readonly IAuthService authService;
        private readonly IWorkflowService workflowService;

        public LoginViewModel(IWorkflowService workflowService, IAuthService authService)
        {
            this.authService = authService;
            this.workflowService = workflowService;
        }

        public async Task Login(string username, string password)
        {
            try
            {
                var studioConfig = new StudioConfig();
                App.Configuration.GetSection(nameof(StudioConfig)).Bind(studioConfig);

                var userCredentials = new UserCredentials
                {
                    UserName = username,
                    Password = password
                };
                var token = await authService.GetToken(userCredentials, studioConfig.TokenApiUrl);

                connection = new HubConnectionBuilder()
                    .WithUrl(studioConfig.WorkflowHubUrl, options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token.AccessToken);
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
            catch (Exception ex)
            {
            }
        }

    }
}
