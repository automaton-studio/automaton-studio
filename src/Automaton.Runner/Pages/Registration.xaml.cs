using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Pages;

public partial class RegistrationControl : Page
{
    public RegistrationControl()
    {
        InitializeComponent();
    }

    private async void RegisterClick(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as RegistrationViewModel;
        var mainWindow = App.Current.MainWindow as MainWindow;

        var result = await viewModel.Register();

        if (result == Enums.RunnerNavigation.Dashboard)
        {
            mainWindow.NavigateToDashboard();
        }
    }
}
