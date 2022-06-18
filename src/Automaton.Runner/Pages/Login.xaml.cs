using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Pages;

public partial class Login : Page
{
    public Login()
    {
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

        EnterApplication();
    }

    private void EnterApplication()
    {
        var viewModel = DataContext as LoginViewModel;
        var mainWindow = Application.Current.MainWindow as MainWindow;

        if (viewModel.IsRunnerRegistered())
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
