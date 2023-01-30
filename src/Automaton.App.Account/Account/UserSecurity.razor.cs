using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Automaton.App.Account.Account;

public partial class UserSecurity : ComponentBase
{
    private bool loading = false;

    [Inject] private UserSecurityViewModel UserSecurityViewModel { get; set; } = default!;
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

            await UserSecurityViewModel.UpdateUserPassword();

            await MessageService.Info("User password updated");
        }
        catch (Exception ex)
        {
            await MessageService.Error("User password update failed");
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
