using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

public partial class SetupDetails : ComponentBase
{
    private bool loading = false;

    [Inject] private SetupViewModel SetupViewModel { get; set; } = default!;
    [Inject] private MessageService MessageService { get; set; }

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
