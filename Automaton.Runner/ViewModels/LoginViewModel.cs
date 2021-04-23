using Automaton.Runner.Core.Services;
using Automaton.Runner.Enums;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels.Common;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private readonly IAppConfigurationService configurationService;
        private readonly IAuthService authService;
        private readonly IHubService hubService;
        private readonly LoginValidator loginValidator;

        #region Properties

        public string UserName { get; set; }
        public string Password { get; set; }
        public IViewModelLoader Loader { get; set; }

        #endregion

        #region Constructors

        public LoginViewModel(
            IAppConfigurationService configService,
            IAuthService authService,
            IHubService hubService,
            IViewModelLoader loader)
        {
            this.configurationService = configService;
            this.authService = authService;
            this.hubService = hubService;
            this.Loader = loader;

            loginValidator = new LoginValidator();
        }

        #endregion

        public async Task<AppNavigate> Login(string username, string password)
        {
            try
            {
                Loader.ClearErrors();

                UserName = username;
                Password = password;

                // Validate login
                var results = loginValidator.Validate(this);
                if (results != null && results.Errors.Any())
                {
                    Loader.SetErrors(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray()));
                    return AppNavigate.None;
                }

                Loader.StartLoading();

                var studioConfig = configurationService.GetStudioConfig();

                // Perform authentication
                var token = await authService.SignIn(username, password, studioConfig.TokenApiUrl);

                var userConfig = configurationService.GetUserConfig();

                if (userConfig.IsRunnerRegistered())
                {
                    // Connect to SignalR server hub
                    await hubService.Connect(token, userConfig.RunnerName);

                    return AppNavigate.Dashboard;
                }
                else
                {
                    return AppNavigate.Registration;
                }
            }
            catch (HttpRequestException httpException)
            {
                // Display generic authetication error
                Loader.SetErrors(Resources.Errors.AuthenticationFail);
            }
            catch (Exception ex)
            {
                // Display generic authetication error
                Loader.SetErrors(string.Join(Environment.NewLine,
                    Resources.Errors.AuthenticationError,
                    Resources.Errors.ContactAdministrator));
            }
            finally
            {
                // Hide Authenticating... message
                Loader.StopLoading();
            }

            return AppNavigate.None;
        }
    }
}
