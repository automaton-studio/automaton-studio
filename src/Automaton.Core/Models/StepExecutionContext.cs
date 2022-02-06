using Automaton.Core.Interfaces;

namespace Automaton.Core.Models
{
    public class StepExecutionContext : IStepExecutionContext
    {
        public WorkflowDefinition WorkflowDefinition { get; set; }

        public WorkflowStep Step { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
