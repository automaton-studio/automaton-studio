using Automaton.Core.Enums;

namespace Automaton.Core.Models
{
    public abstract class WorkflowStep
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public string Type { get; set; }

        public string? NextStepId { get; set; }

        public virtual List<WorkflowStep> Children { get; set; } = new List<WorkflowStep>();

        public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();
        
        public IDictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

        public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

        public virtual TimeSpan? RetryInterval { get; set; }

        public abstract Task<ExecutionResult> RunAsync(StepExecutionContext context);
    }
}
