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

        var result = await viewModel.Login();

        if (viewModel.HasErrors)
        {
            ErrorsSnackbar.Show();
            return;
        }

        var mainWindow = App.Current.MainWindow as MainWindow;

        if (result == Enums.RunnerNavigation.Dashboard)
            mainWindow.NavigateToDashboard();
        else if (result == Enums.RunnerNavigation.Registration)
            mainWindow.NavigateToRegistration();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        var viewModel = DataContext as LoginViewModel;
        viewModel.Password = passwordBox.Password;
    }
}
