using Automaton.Runner.Core;
using Automaton.Runner.Core.Resources;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Enums;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels.Common;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private readonly ConfigService configService;
        private readonly IAuthService authService;
        private readonly IHubService hubService;
        private readonly LoginValidator loginValidator;

        #region Properties

        public IViewModelLoader Loader { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        #endregion

        #region Constructors

        public LoginViewModel(
            ConfigService configService,
            IAuthService authService,
            IHubService hubService,
            IViewModelLoader loader,
            LoginValidator loginValidator)
        {
            this.configService = configService;
            this.authService = authService;
            this.hubService = hubService;
            this.Loader = loader;
            this.loginValidator = loginValidator;
        }

        #endregion

        public async Task<RunnerNavigation> Login()
        {
            try
            {
                if (!Validate())
                {
                    return RunnerNavigation.None;
                }

                Loader.StartLoading();

                // Authenticate before connecting to the hub service
                await authService.SignIn(UserName, Password);

                if (configService.UserConfig.IsRunnerRegistered())
                {
                    // Connect to the hub service
                    await hubService.Connect(authService.Token, configService.UserConfig.RunnerName);

                    return RunnerNavigation.Dashboard;
                }
                else
                {
                    return RunnerNavigation.Registration;
                }
            }
            catch (AuthenticationException ex)
            {
                Loader.SetErrors(Errors.AuthenticationError);
            }
            catch (Exception ex)
            {
                Loader.SetErrors(Errors.ApplicationError);
            }
            finally
            {
                Loader.StopLoading();
            }

            return RunnerNavigation.None;
        }

        private bool Validate()
        {
            Loader.ClearErrors();

            var results = loginValidator.Validate(this);

            if (results != null && results.Errors.Any())
            {
                Loader.SetErrors(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray()));
                
                return false;
            }

            return true;
        }
    }
}
