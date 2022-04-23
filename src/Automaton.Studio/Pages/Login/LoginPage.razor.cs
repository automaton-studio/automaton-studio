using Automaton.Studio.Services;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    partial class LoginPage : ComponentBase
    {
        private readonly bool loading = false;
        private FluentValidationValidator fluentValidator;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private LoginViewModel LoginViewModel { get; set; } = default!;
        [Inject] public NavMenuService NavMenuService { get; set; }

        public LoginModel Model => LoginViewModel.LoginModel;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFinish(EditContext editContext)
        {
            var result = await LoginViewModel.Login();

            if (result)
            {
                NavigationManager.NavigateTo($"/");
            }
        }

        private void OnFinishFailed(EditContext editContext)
        {
        }
    }
}
