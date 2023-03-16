using Automaton.Core.Models;
using Automaton.Core.Scripting;

namespace Automaton.Steps;

public class CustomStep : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    public string? Code { get; set; }

    public IList<StepVariable> CodeInputVariables { get; set; }

    public IList<StepVariable> CodeOutputVariables { get; set; }

    public CustomStep(ScriptEngineHost scriptHost)
    {
        this.scriptHost = scriptHost;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var resource = new ScriptResource()
        {
            ContentType = ContentType,
            Content = Code
        };

        var inputVariablesDictionary = CodeInputVariables.ToDictionary(x => x.Name, x => (object)x.Value);

        var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

        foreach (var variable in CodeOutputVariables)
        {
            if (scriptVariables.ContainsKey(variable.Name))
            {
                variable.Value = scriptVariables[variable.Name].ToString();
                context.Workflow.Variables[variable.Name] = variable.Value;
            }
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
