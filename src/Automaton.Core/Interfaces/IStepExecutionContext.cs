using Automaton.Core.Models;

namespace Automaton.Core.Interfaces
{
    public interface IStepExecutionContext
    {
        Workflow Workflow { get; set; }

        WorkflowStep Step { get; set; }

        WorkflowDefinition Definition { get; set; }

        CancellationToken CancellationToken { get; set; }
    }
}