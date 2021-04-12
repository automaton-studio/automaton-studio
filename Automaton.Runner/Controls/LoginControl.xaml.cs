using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            await viewModel.Login(UsernameBox.Text, PasswordBox.Password);
        }
    }
}
