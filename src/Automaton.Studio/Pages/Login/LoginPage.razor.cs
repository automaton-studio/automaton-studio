using Automaton.Studio.Components;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    partial class LoginPage : ComponentBase
    {
        private readonly bool loading = false;
        private ServerSideValidator serverSideValidator;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private LoginViewModel LoginViewModel { get; set; } = default!;
        [Inject] public NavMenuService NavMenuService { get; set; }

        public LoginModel Model => LoginViewModel.LoginCredentials;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFinish(EditContext editContext)
        {
            var success = await LoginViewModel.Login();

            if (success)
            {
                NavigationManager.NavigateTo($"/");
            }

            serverSideValidator.DisplayErrors(LoginViewModel.Errors);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            // Do nothing if form validation fails
        }
    }
}
