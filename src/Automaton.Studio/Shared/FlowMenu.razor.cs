using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class FlowMenu : ComponentBase
    {
        [Parameter] public Guid FlowId { get; set; }
        [Parameter] public string FlowName { get; set; }
        [Parameter] public bool Collapsed { get; set; }

        public string FlowDesignerUrl => $"flowdesigner/{FlowId}/{FlowName}";
        public string FlowActivityUrl => $"flowactivity/{FlowId}/{FlowName}";
        public string FlowLogsUrl => $"flowlogs/{FlowId}/{FlowName}";
        public string FlowScheduleUrl => $"flowschedule/{FlowId}/{FlowName}";
    }
}
