using AntDesign;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login;

partial class LoginPage : ComponentBase
{
    private bool loading = false;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private LoginViewModel LoginViewModel { get; set; } = default!;
    [Inject] private MessageService MessageService { get; set; }
    [Inject] private ConfigurationService ConfigurationService { get; set; }

    public LoginModel Model => LoginViewModel.LoginDetails;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public void RegisterUser()
    {
        NavigationManager.NavigateTo($"/register");
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await LoginViewModel.Login();
        }
        catch (Exception ex)
        {
            await MessageService.Error(Resources.Errors.LoginFailed);
        }
        finally
        {
            loading = false;
        }

        NavigationManager.NavigateTo($"/");
    }

    private void OnFinishFailed(EditContext editContext)
    {
        Task.Run(() => MessageService.Error(Resources.Errors.LoginFailed));
    }
}
