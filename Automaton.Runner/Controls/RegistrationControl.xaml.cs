using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls
{
    /// <summary>
    /// Runner registration control
    /// </summary>
    public partial class RegistrationControl : UserControl
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

            if (result == Enums.AppNavigate.Dashboard)
                mainWindow.NavigateToDashboard();
            
        }
    }
}
