using Automaton.Runner.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls;

/// <summary>
/// Runner registration control
/// </summary>
public partial class DashboardControl : UserControl
{
    private DashboardViewModel viewModel;

    public DashboardControl()
    {
        InitializeComponent();
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        viewModel = DataContext as DashboardViewModel;

        var connected = await viewModel.ConnectToHub();
    }
}
