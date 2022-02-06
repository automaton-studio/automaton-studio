using Automaton.Core.Models;

namespace Automaton.Core.Interfaces
{
    public interface IWorkflowExecutor
    {
        Task<WorkflowExecutorResult> Execute(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
    }
}