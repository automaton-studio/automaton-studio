using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Automaton.Studio.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        private NavMenu NavigationMenu { get; set; }

        [Inject] JsInterop JsInterop { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] MainLayoutViewModel MainLayoutViewModel { get; set; }
        [Inject] KeyboardService KeyboardService { get; set; }

        private void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
            NavigationMenu.SetCollapsed(MenuCollapsed);
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

        private void OnKeyDown(KeyboardEventArgs e)
        {
            KeyboardService.KeyDown(e.Code);
        }

        private void OnKeyUp(KeyboardEventArgs e)
        {
            KeyboardService.KeyUp(e.Code);
        }
    }
}
