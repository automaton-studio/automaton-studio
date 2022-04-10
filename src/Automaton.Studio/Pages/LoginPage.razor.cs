using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class LoginPage : ComponentBase
    {
        private bool loading = false;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ILoginViewModel LoginViewModel { get; set; } = default!;
        [Inject] public INavMenuService NavMenuService { get; set; }
        [Inject] public IAuthenticationService AuthenticationService { get; set; }


        public LoginModel Model => LoginViewModel.Model;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFinish(EditContext editContext)
        {
            var result = await AuthenticationService.Login(Model);

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
