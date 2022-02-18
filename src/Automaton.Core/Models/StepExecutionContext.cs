using Automaton.Core.Interfaces;

namespace Automaton.Core.Models
{
    public class StepExecutionContext : IStepExecutionContext
    {
        public Workflow Workflow { get; set; }

        public WorkflowDefinition Definition { get; set; }

        public WorkflowStep Step { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
