using Automaton.Core.Models;

namespace Automaton.Steps;

public class EmitLog : WorkflowStep
{
    public string Message { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        logger.Information("Write message: {0}", Message);

        return Task.FromResult(ExecutionResult.Next());
    }
}
