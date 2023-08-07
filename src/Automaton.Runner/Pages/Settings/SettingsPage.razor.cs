using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Settings;

partial class SettingsPage : ComponentBase
{
    [Inject] private SettingsViewModel SetupViewModel { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SetupViewModel.LoadSettings();
    }
}
