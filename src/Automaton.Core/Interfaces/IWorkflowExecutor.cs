using Automaton.Core.Models;

namespace Automaton.Core.Interfaces
{
    public interface IWorkflowExecutor
    {
        Task<WorkflowExecutorResult> Execute(Workflow workflow, CancellationToken cancellationToken = default);
    }
}