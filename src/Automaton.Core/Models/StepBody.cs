using System;
using System.Threading.Tasks;
using Automaton.Core.Interfaces;

namespace Automaton.Core.Models
{
    public abstract class StepBody : IStepBody
    {
        public abstract ExecutionResult Run(IStepExecutionContext context);

        public Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return Task.FromResult(Run(context));
        }        
    }
}
