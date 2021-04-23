using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAppConfigurationService configurationService;
        private readonly IAuthService authService;
        private readonly IHubService hubService;
        private readonly LoginValidator loginValidator;

        #region Properties

        public string UserName { get; set; }
        public string Password { get; set; }

        private string errors;
        public string Errors
        {
            get => errors;
            set
            {
                errors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        private bool authenticating;
        public bool Authenticating
        {
            get => authenticating;
            set
            {
                authenticating = value;
                OnPropertyChanged();
            }
        }

        public bool HasErrors
        {
            get => !string.IsNullOrEmpty(Errors);
        }

        #endregion

        #region Constructors

        public LoginViewModel(
            IAppConfigurationService configService,
            IAuthService authService,
            IHubService hubService)
        {
            this.configurationService = configService;
            this.authService = authService;
            this.hubService = hubService;
            loginValidator = new LoginValidator();
        }

        #endregion

        public async Task Login(string username, string password)
        {
            try
            {
                UserName = username;
                Password = password;
                Errors = string.Empty;

                var results = loginValidator.Validate(this);
                if (results != null && results.Errors.Any())
                {
                    Errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                    return;
                }

                Authenticating = true;

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
                Errors = Resources.Errors.AuthenticationFail;
            }
            catch (Exception ex)
            {
                Errors = string.Join(Environment.NewLine,
                    Resources.Errors.AuthenticationError,
                    Resources.Errors.ContactAdministrator);
            }
            finally
            {
                Authenticating = false;
            }
        }

        #region Validation

        public string this[string columnName]
        {
            get
            {
                var firstOrDefault = loginValidator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                
                return firstOrDefault != null ? 
                    loginValidator != null ? firstOrDefault.ErrorMessage : string.Empty : 
                    string.Empty;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
