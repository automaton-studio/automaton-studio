using Automaton.Core.Interfaces;
using WorkflowCore.Models;

namespace Automaton.Core.Models
{
    public class StepExecutionContext : IStepExecutionContext
    {
        public WorkflowInstance Workflow { get; set; }

        public WorkflowStep Step { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
