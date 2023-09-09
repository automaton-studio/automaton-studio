using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class FlowMenu : ComponentBase
    {
        [Parameter] public bool Collapsed { get; set; }
    }
}
