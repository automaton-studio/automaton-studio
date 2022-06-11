using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Automaton.Runner;

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

        Closing += WindowClosing;

        InitializeComponent();
    }

    protected override async void OnInitialized(EventArgs e)
    {
        if (await IsAuthenticated())
        {
            if (IsRunnerRegistered())
            {
                NavigateToDashboard();
            }
            else
            {
                NavigateToRegistration();
            }
        }
        else
        {
            NavigateToLogin();
        }

        base.OnInitialized(e);
    }

    public void NavigateToLogin()
    {
        RootNavigation.Navigate("login");
    }

    public void NavigateToRegistration()
    {
        RootNavigation.Navigate("dashboard");
    }

    public void NavigateToDashboard()
    {
        RootNavigation.Navigate("dashboard");
    }

    private void Logout_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private async Task<bool> IsAuthenticated()
    {
        var authenticated = await authenticationService.IsAuthenticated();

        return authenticated;
    }

    private bool IsRunnerRegistered()
    {
        var registered = configService.AppConfig.IsRunnerRegistered();

        return registered;
    }

    private async void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await hubService.Disconnect();
    }
}
