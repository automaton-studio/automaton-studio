namespace Automaton.Core.Models
{
    public class StepExecutionContext
    {
        public Workflow Workflow { get; set; }

        public WorkflowDefinition Definition { get; set; }

        public WorkflowStep Step { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
