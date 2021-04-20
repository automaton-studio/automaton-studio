using Automaton.Runner.Core;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private readonly IAppConfigurationService configurationService;
        private readonly IAuthService authService;
        private readonly IHubService hubService;

        public LoginViewModel(
            IAppConfigurationService configService,
            IAuthService authService,
            IHubService hubService)
        {
            this.configurationService = configService;
            this.authService = authService;
            this.hubService = hubService;
        }

        public async Task Login(string username, string password)
        {
            try
            {
                var studioConfig = configurationService.GetStudioConfig();
                var token = await authService.SignIn(username, password, studioConfig.TokenApiUrl);

                var mainWindow = App.Current.MainWindow as MainWindow;
                var userConfig = configurationService.GetUserConfig();

                if (userConfig.IsRunnerRegistered())
                {
                    await hubService.Connect(token, userConfig.RunnerName);

                    mainWindow.ShowDashboardControl();
                }
                else
                {
                    mainWindow.ShowRegistrationControl();
                }
            }
            catch (HttpRequestException httpException)
            {        
            }
            catch (Exception ex)
            {
            }
        }
    }
}
