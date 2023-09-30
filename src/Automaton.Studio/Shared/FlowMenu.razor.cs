using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class FlowMenu : ComponentBase
    {
        [Parameter] public Guid FlowId { get; set; }
        [Parameter] public string FlowName { get; set; }
        [Parameter] public bool Collapsed { get; set; }

        public string FlowDesignerUrl => $"flow/designer/{FlowId}/{FlowName}";
        public string FlowActivityUrl => $"flow/activity/{FlowId}/{FlowName}";
        public string FlowLogsUrl => $"flow/logs/{FlowId}/{FlowName}";
        public string FlowScheduleUrl => $"flow/schedule/{FlowId}/{FlowName}";
    }
}
