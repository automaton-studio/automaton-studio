using Automaton.Runner.Core.Resources;
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
    public class RegistrationViewModel
    {
        private readonly IHubService hubService;
        private readonly IAuthService authService;
        private readonly IRegistrationService registrationService;
        private readonly RegistrationValidator registrationValidator;
        private readonly ConfigService configService;

        #region Properties

        public IViewModelLoader Loader { get; set; }
        public string RunnerName { get; set; }

        #endregion

        public RegistrationViewModel
        (
            IHubService hubService, 
            IAuthService authService,
            IViewModelLoader loader,
            IRegistrationService registrationService,
            ConfigService configService,
            RegistrationValidator registrationValidator
        )
        {
            this.hubService = hubService;
            this.authService = authService;
            this.Loader = loader;
            this.configService = configService;
            this.registrationService = registrationService;
            this.registrationValidator = registrationValidator;
        }

        public async Task<RunnerNavigation> Register()
        {
            try
            {
                if (!Validate())
                {
                    return RunnerNavigation.None;
                }

                Loader.StartLoading();

                await registrationService.Register(RunnerName);

                configService.RegisterRunner(RunnerName);

                await hubService.Connect(authService.Token, RunnerName);

                return RunnerNavigation.Dashboard;
            }
            catch (HttpRequestException ex)
            {
                Loader.SetErrors(ex.Message);
            }
            catch
            {
                Loader.SetErrors(Errors.RegistrationError);
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

            var results = registrationValidator.Validate(this);

            if (results != null && results.Errors.Any())
            {
                Loader.SetErrors(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray()));

                return false;
            }

            return true;
        }
    }
}
