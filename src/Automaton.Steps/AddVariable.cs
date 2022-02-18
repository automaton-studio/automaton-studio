using Automaton.Core.Interfaces;
using Automaton.Core.Models;

namespace Automaton.Steps
{
    public class AddVariable : WorkflowStep
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public AddVariable()
        {
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            context.Workflow.VariablesDictionary.Add(Name, Value);

            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
