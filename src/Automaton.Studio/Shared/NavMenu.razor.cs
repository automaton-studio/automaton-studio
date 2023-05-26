using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Parameter] public bool Collapsed { get; set; }
    }
}
