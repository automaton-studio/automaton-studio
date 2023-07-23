using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] HttpClient HttpClient { get; set; }

        void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }

        public void Logout()
        {
        }
    }
}
