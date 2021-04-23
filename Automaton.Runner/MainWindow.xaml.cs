using Automaton.Runner.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Allow user to drag the main window around
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public void NavigateToRegistration()
        {
            frame.NavigationService.Navigate(new Uri("Controls/RegistrationControl.xaml", UriKind.Relative));
        }

        public void NavigateToDashboard()
        {
            frame.NavigationService.Navigate(new Uri("Controls/DashboardControl.xaml", UriKind.Relative));
        }
    }
}
