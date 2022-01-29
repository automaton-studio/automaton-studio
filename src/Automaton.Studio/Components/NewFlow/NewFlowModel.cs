using System;

namespace Automaton.Studio.Components.NewFlow
{
    public class NewFlowModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string StartupWorkflowId { get; set; }
    }
}
