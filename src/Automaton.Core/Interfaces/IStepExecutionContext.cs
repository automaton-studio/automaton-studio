using Automaton.Core.Models;

namespace Automaton.Core.Interfaces
{
    public interface IStepExecutionContext
    {
        WorkflowStep Step { get; set; }

        WorkflowDefinition WorkflowDefinition { get; set; }

        CancellationToken CancellationToken { get; set; }
    }
}