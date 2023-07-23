using Microsoft.AspNetCore.Components;

namespace Automaton.Runner.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Parameter] public bool Collapsed { get; set; }
    }
}
