using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Settings;

public partial class Settings : ComponentBase
{
    private bool loading = false;

    [Parameter] public SettingsViewModel SettingsViewModel { get; set; }

    [Inject] private MessageService MessageService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await SettingsViewModel.SaveSettings();

            await MessageService.Info("Runner settings updated");
        }
        catch (Exception ex)
        {
            await MessageService.Error("Runner settings update failed");
        }
        finally
        {
            loading = false;
        }
    }

    private void OnFinishFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }
}
