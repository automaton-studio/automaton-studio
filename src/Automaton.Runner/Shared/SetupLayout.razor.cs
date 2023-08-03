using Microsoft.AspNetCore.Components;

namespace Automaton.Runner.Shared
{
    public partial class SetupLayout : LayoutComponentBase
    {
        private bool MenuCollapsed { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; } = default!;

        void ToggleCollapsed()
        {
            MenuCollapsed = !MenuCollapsed;
        }
    }
}
