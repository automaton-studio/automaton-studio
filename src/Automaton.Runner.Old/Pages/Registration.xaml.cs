using Automaton.Runner.ViewModels;

namespace Automaton.Runner.Pages;

public partial class Registration : Page
{
    public Registration()
    {
        InitializeComponent();
    }

    private async void RegisterClick(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as RegistrationViewModel;
        var mainWindow = Application.Current.MainWindow as MainWindow;

        var registered = await viewModel.Register();

        if (!registered)
        {
            ErrorsSnackbar.Show();
            return;
        }
       
        mainWindow.NavigateToDashboard();
    }
}
