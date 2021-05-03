using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    partial class MainLayout : IDisposable
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        [CascadingParameter] protected Task<AuthenticationState>? AuthStat { get; set; }

        protected async override Task OnInitializedAsync()
        {
            base.OnInitialized();

            await ValidateUserAuthentication();

            NavigationManager.LocationChanged += LocationChanged;

            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(NavigationManager.Uri + "/designer");
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

        private void LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(NavigationManager.Uri);
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
