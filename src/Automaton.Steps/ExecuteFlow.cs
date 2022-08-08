using Automaton.Core.Models;

namespace Automaton.Steps;

public class ExecuteFlow : WorkflowStep
{
    public string Name { get; set; }

    public ExecuteFlow()
    {
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
       
        return Task.FromResult(ExecutionResult.Next());
    }
}
