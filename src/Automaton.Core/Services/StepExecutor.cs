using Automaton.Core.Interfaces;
using Automaton.Core.Models;

namespace Automaton.Core.Services
{
    public class StepExecutor : IStepExecutor
    {
        /// <summary>
        /// Runs the passed <see cref="IStepBody"/> in the given <see cref="IStepExecutionContext"/>
        /// </summary>
        /// <param name="context">The <see cref="IStepExecutionContext"/> in which to execute the step.</param>
        /// <param name="body">The <see cref="IStepBody"/> body.</param>
        /// <returns>A <see cref="Task{ExecutionResult}"/> to wait for the result of running the step</returns>
        public async Task<ExecutionResult> ExecuteStep(
            IStepExecutionContext context,
            IStepBody body
        )
        {
            return await body.RunAsync(context);
        }
    }
}
