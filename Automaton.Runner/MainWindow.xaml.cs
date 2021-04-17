using Automaton.Runner.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

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

        public void ShowRegistrationControl()
        {
            ViewModel.ShowRegistrationControl();
        }

        public void ShowDashboardControl()
        {
            ViewModel.ShowDashboardControl();
        }
    }
}
