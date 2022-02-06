using Automaton.Core.Interfaces;

namespace Automaton.Core.Models
{
    public abstract class StepBodyAsync : IStepBody
    {
        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
