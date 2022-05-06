using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    partial class LoginPage : ComponentBase
    {
        private readonly bool loading = false;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private LoginViewModel LoginViewModel { get; set; } = default!;
        [Inject] private MessageService MessageService { get; set; }

        public LoginModel Model => LoginViewModel.LoginDetails;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFinish(EditContext editContext)
        {
            try
            {
                await LoginViewModel.Login();
            }
            catch (Exception ex)
            {
                await MessageService.Error(Resources.Errors.LoginFailed);
            }

            NavigationManager.NavigateTo($"/");
        }

        private void OnFinishFailed(EditContext editContext)
        {
            // Do nothing if form validation fails
        }
    }
}
