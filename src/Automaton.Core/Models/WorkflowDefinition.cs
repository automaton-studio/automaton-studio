using Automaton.Core.Enums;

namespace Automaton.Core.Models
{
    public class WorkflowDefinition
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public WorkflowStepCollection Steps { get; set; } = new WorkflowStepCollection();

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

    }
}
