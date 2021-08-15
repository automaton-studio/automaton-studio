using Elsa.Models;
using System;

namespace Automaton.Studio.Entities
{
    public class FlowWorkflow
    {
        public Guid FlowId { get; set; }
        public string WorkflowId { get; set; }

        public virtual Flow Flow { get; set; }
        public virtual WorkflowDefinition Workflow { get; set; }
    }
}
