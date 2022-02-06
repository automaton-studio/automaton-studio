using Automaton.Core.Models;
using WorkflowCore.Models;

namespace Automaton.Core.Interfaces
{
    public interface IStepExecutionContext
    {
        WorkflowStep Step { get; set; }

        WorkflowInstance Workflow { get; set; }

        CancellationToken CancellationToken { get; set; }
    }
}