using Automaton.Core.Interfaces;
using WorkflowCore.Models;

namespace Automaton.Core.Models
{
    public abstract class StepBodyAsync : IStepBody
    {
        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
