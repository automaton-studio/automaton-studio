using AntDesign;
using Automaton.App.Authentication.Config;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Automaton.App.Authentication.Pages.Register;

partial class UserRegisterPage : ComponentBase
{
    private bool loading = false;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private UserRegisterViewModel UserRegisterViewModel { get; set; } = default!;
    [Inject] private MessageService MessageService { get; set; }
    [Inject] private ConfigurationService ConfigurationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (ConfigurationService.NoUserSignUp)
        {
            NavigationManager.NavigateTo("/registernotavailable");
        }

        await base.OnInitializedAsync();
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await UserRegisterViewModel.Register();

            NavigationManager.NavigateTo("/");
        }
        catch (Exception ex)
        {
            await MessageService.Error(Resources.Errors.UserRegistrationFailed);
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
