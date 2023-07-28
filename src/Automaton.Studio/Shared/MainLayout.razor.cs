using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] JsInterop JsInterop { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] MainLayoutViewModel MainLayoutViewModel { get; set; }

        private void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }

        private async Task ToggleFullScreen()
        {
            await JsInterop.ToggleFullscreen();
        }

        private async Task Logout()
        {
            await MainLayoutViewModel.Logout();

            NavigationManager.NavigateTo($"/");
        }
    }
}
