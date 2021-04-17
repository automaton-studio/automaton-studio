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

            await viewModel.Register(RunnerNameBox.Text);
        }
    }
}
