using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    partial class MainLayout
    {
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        [CascadingParameter] protected Task<AuthenticationState>? AuthStat { get; set; }

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
    }
}
