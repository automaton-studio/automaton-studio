using Automaton.Runner.ViewModels;

namespace Automaton.Runner.Pages;

public partial class Settings : Page
{
    private DashboardViewModel viewModel;

    public Settings()
    {
        InitializeComponent();
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        viewModel = DataContext as DashboardViewModel;
    }
}
