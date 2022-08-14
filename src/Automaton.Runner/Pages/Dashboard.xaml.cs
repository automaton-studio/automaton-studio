using Automaton.Runner.ViewModels;
using System;
using System.Windows.Controls;

namespace Automaton.Runner.Pages;

public partial class Dashboard : Page
{
    private DashboardViewModel viewModel;

    public Dashboard()
    {
        InitializeComponent();
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        viewModel = DataContext as DashboardViewModel;

        await viewModel.ConnectToServer();
    }
}
