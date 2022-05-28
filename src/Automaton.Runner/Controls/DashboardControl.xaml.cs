using Automaton.Runner.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls;

/// <summary>
/// Runner registration control
/// </summary>
public partial class DashboardControl : UserControl
{
    public DashboardControl()
    {
        var viewModel = DataContext as DashboardViewModel;

        InitializeComponent();
    }
}
