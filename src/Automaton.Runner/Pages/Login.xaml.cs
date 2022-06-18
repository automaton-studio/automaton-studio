using Automaton.Runner.Core.Services;
using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Pages;

public partial class Login : Page
{
    private readonly ConfigService configService;

    public Login(ConfigService configService)
    {
        this.configService = configService;

        InitializeComponent();
    }

    private async void LoginClick(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as LoginViewModel;

        var authenticated = await viewModel.Login();

        if (!authenticated)
        {
            ErrorsSnackbar.Show();
            return;
        }

        var mainWindow = Application.Current.MainWindow as MainWindow;

        if (configService.AppConfig.IsRunnerRegistered())
        {
            mainWindow.NavigateToDashboard();
        }
        else
        {
            mainWindow.NavigateToRegistration();
        }
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        var viewModel = DataContext as LoginViewModel;

        viewModel.Password = passwordBox.Password;
    }
}
