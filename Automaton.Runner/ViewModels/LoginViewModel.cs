using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAppConfigurationService configurationService;
        private readonly IAuthService authService;
        private readonly IHubService hubService;
        private readonly LoginValidator loginValidator;

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private string errors;
        public string Errors
        {
            get { return errors; }
            set
            {
                errors = value;
                OnPropertyChanged("Errors");
                OnPropertyChanged("HasErrors");
            }
        }

        private bool authenticating;
        public bool Authenticating
        {
            get { return authenticating; }
            set
            {
                authenticating = value;
                OnPropertyChanged("Authenticating");
            }
        }

        public bool HasErrors
        {
            get { return !string.IsNullOrEmpty(Errors); }
        }

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
                Errors = "Can not authenticate to the server.";
            }
            catch (Exception ex)
            {
                Errors = string.Join(Environment.NewLine, 
                    "A problem has occured during authentication.",
                    "Please contact administrator.");
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
                if (firstOrDefault != null)
                    return loginValidator != null ? firstOrDefault.ErrorMessage : "";
                return "";
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
