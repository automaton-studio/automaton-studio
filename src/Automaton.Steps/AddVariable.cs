using Automaton.Core.Interfaces;
using Automaton.Core.Models;

namespace Automaton.Steps
{
    public class AddVariable : WorkflowStep
    {
        public string VariableName { get; set; }
        public string VariableValue { get; set; }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            context.Workflow.VariablesDictionary.Add(VariableName, VariableValue);

            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
