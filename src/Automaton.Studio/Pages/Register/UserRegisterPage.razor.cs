using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Register;

partial class UserRegisterPage : ComponentBase
{
    private bool loading = false;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private UserRegisterViewModel UserRegisterViewModel { get; set; } = default!;
    [Inject] private MessageService MessageService { get; set; }

    public UserRegisterModel Model => UserRegisterViewModel.UserRegisterDetails;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await UserRegisterViewModel.Register();
        }
        catch (Exception ex)
        {
            await MessageService.Error(Resources.Errors.UserRegistrationFailed);
        }
        finally
        {
            loading = false;
        }

        NavigationManager.NavigateTo($"/");
    }

    private void OnFinishFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }
}
