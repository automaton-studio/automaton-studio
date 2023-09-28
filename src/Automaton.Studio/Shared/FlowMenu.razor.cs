using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class FlowMenu : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }
        [Parameter] public bool Collapsed { get; set; }

        public string FlowDesignerUrl => $"flowdesigner/{Id}";
        public string FlowActivityUrl => $"flowactivity/{Id}";
        public string FlowLogUrl => $"flowlog/{Id}";
        public string FlowScheduleUrl => $"flowschedule/{Id}";
    }
}
