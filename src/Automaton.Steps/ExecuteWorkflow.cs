using Automaton.Core.Models;

namespace Automaton.Steps;

public class ExecuteWorkflow : WorkflowStep
{
    public string Name { get; set; }

    public ExecuteWorkflow()
    {
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
       
        return Task.FromResult(ExecutionResult.Next());
    }
}
