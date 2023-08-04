using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

public partial class SetupDetails : ComponentBase
{
    private bool loading = false;

    [Parameter] public SetupViewModel SetupViewModel { get; set; }

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

            await SetupViewModel.RegisterRunner();

            await MessageService.Info("Runner Registered");

            NavigationManager.NavigateTo($"/");
        }
        catch (Exception ex)
        {
            await MessageService.Error("Runner registration failed");
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
