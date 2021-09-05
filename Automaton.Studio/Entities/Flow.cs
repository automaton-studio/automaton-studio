using System;
using System.Collections.Generic;

namespace Automaton.Studio.Entities
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupWorkflowId { get; set; } = Guid.NewGuid().ToString();

        public virtual IEnumerable<FlowUser> FlowUsers { get; set; }
        public virtual IEnumerable<FlowWorkflow> FlowWorkflows { get; set; }
    }
}
