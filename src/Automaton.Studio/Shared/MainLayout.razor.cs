using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] HttpClient HttpClient { get; set; }

        [Inject] JsInterop JsInterop { get; set; }

        [Inject] IAuthenticationStorage AuthenticationStorage { get; set; }

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

        void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }

        public async Task ToggleFullScreen()
        {
            await JsInterop.ToggleFullscreen();
        }

        public async Task Logout()
        {
            await AuthenticationStorage.DeleteJsonWebToken();

            ((AuthStateProvider)AuthStateProvider).NotifyUserLogout();
            HttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
