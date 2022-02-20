using Automaton.Core.Enums;
using Automaton.Core.Interfaces;
using System.Dynamic;

namespace Automaton.Core.Models
{
    public abstract class WorkflowStep
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public string Type { get; set; }

        public string? NextStepId { get; set; }

        public virtual List<WorkflowStep> Children { get; set; } = new List<WorkflowStep>();

        public ExpandoObject Inputs { get; set; } = new ExpandoObject();

        public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

        public virtual TimeSpan? RetryInterval { get; set; }

        public IList<string>? Variables { get; set; }

        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
