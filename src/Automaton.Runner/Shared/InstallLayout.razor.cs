using Microsoft.AspNetCore.Components;

namespace Automaton.Runner.Shared
{
    public partial class InstallLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; } = default!;

        void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }
    }
}
