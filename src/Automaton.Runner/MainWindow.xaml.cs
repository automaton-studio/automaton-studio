using Automaton.Runner.Enums;
using Automaton.Runner.ViewModels;
using System;
using System.Windows;

namespace Automaton.Runner;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; private set; }

    public MainWindow()
    {
        Closing += WindowClosing;

        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        ViewModel = DataContext as MainWindowViewModel;

        ViewModel.InitializeNavigation();

        if (ViewModel.IsAuthenticated())
        {
            if (ViewModel.IsRunnerRegistered())
            {
                NavigateToDashboard();
            }
            else
            {
                NavigateToRegistration();
            }
        }
        else
        {
            NavigateToLogin();
        }

        base.OnInitialized(e);
    }

    public void NavigateToLogin()
    {
        ViewModel.ApplyLoginMenuVisibility();
        NavigateTo(RunnerPages.Login);
    }

    public void NavigateToRegistration()
    {
        ViewModel.ApplyRegistrationMenuVisibility();
        NavigateTo(RunnerPages.Registration);
    }

    public void NavigateToDashboard()
    {
        ViewModel.ApplyHomeMenuVisibility();
        NavigateTo(RunnerPages.Dashboard);
    }

    private void NavigateTo(RunnerPages page)
    {
        RootNavigation.Navigate(page.ToString());
        RootNavigation.SelectedPageIndex = (sbyte)page;
    }

    private async void LogoutClick(object sender, RoutedEventArgs e)
    {
        await ViewModel.Logout();
        NavigateToLogin();
    }

    private async void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await ViewModel.Disconnect();
    }
}
