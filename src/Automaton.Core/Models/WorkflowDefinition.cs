using Automaton.Core.Enums;

namespace Automaton.Core.Models
{
    public class WorkflowDefinition
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public List<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

    }
}
