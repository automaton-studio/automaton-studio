using Automaton.Client.Auth.Services;
using Automaton.Runner.Core.Services;
using Automaton.Runner.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HubService hubService;
        private readonly ConfigService configService;
        private readonly AuthenticationService authenticationService;

        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        public MainWindow(HubService hubService, ConfigService configService, AuthenticationService authenticationService)
        {
            this.hubService = hubService;
            this.configService = configService;
            this.authenticationService = authenticationService;

            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            var loggedIn = await authenticationService.InitLoggedInAuthorization();

            if (loggedIn)
            {
                await hubService.Connect(configService.AppConfig.RunnerName);
                frame.Source = new Uri("Controls/DashboardControl.xaml", UriKind.Relative);
            }
            else
            {
                frame.Source = new Uri("Controls/LoginControl.xaml", UriKind.Relative);
            }

            base.OnInitialized(e);
        }

        public void NavigateToRegistration()
        {
            frame.NavigationService.Navigate(new Uri("Controls/RegistrationControl.xaml", UriKind.Relative));
        }

        public void NavigateToDashboard()
        {
            frame.NavigationService.Navigate(new Uri("Controls/DashboardControl.xaml", UriKind.Relative));
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Allow user to drag the main window around
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            hubService.Disconnect();
        }
    }
}
