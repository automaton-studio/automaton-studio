using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    partial class MainLayout
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await ValidateUserAuthentication();
        }

        private async Task ValidateUserAuthentication()
        {
            if (AuthStat == null)
            {
                throw new ArgumentException("Invalid AuthStat");
            }

            var user = (await AuthStat).User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"Identity/Account/Login");
            }
        }

        private async Task Logout()
        {
            await JSRuntime.InvokeAsync<string>("logout");
        }
    }
}
