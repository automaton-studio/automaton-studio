using Automaton.App.Authentication.Services;
using Automaton.Runner.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Runner.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] MainLayoutViewModel MainLayoutViewModel { get; set; }
        [Inject] ConfigService ConfigService { get; set; }
        [Inject] AuthenticationService AuthenticationService { get; set; }

        public bool IsRunnerRegistered()
        {
            var registered = ConfigService.IsRunnerRegistered();

            return registered;
        }

        void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }

        public async Task Logout()
        {
            await AuthenticationService.Logout();

            NavigationManager.NavigateTo($"/");
        }
    }
}
