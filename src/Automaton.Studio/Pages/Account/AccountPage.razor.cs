using AntDesign;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

partial class AccountPage : ComponentBase
{
    private bool loading = false;

    [Inject] private AccountViewModel AccountViewModel { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public NavMenuService NavMenuService { get; set; }
    [Inject] private MessageService MessageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task Logout()
    {
        await AccountViewModel.Logout();

        NavMenuService.DisableDesignerMenu();

        NavigationManager.NavigateTo($"/");
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await AccountViewModel.UpdateUserProfile();
        }
        catch (Exception ex)
        {
            await MessageService.Error(Resources.Errors.UserProfileUpdateFailed);
        }
        finally
        {
            loading = false;

            await MessageService.Info(Resources.Information.UserProfileUpdated);
        }
    }

    private void OnFinishFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }
}
