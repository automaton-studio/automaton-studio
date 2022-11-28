using Automaton.Core.Models;

namespace Automaton.Steps;

public class AddVariable : WorkflowStep
{
    public string VariableName { get; set; }
    public string VariableValue { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
 