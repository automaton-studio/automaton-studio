using Automaton.Core.Attributes;
using Automaton.Core.Models;
using Automaton.Core.Scripting;

namespace Automaton.Steps;

public class ExecutePython : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    [IgnorePropertyParsing(true)]
    public string? Code { get; set; }

    [IgnorePropertyParsing(true)]
    public IList<StepVariable> CodeInputVariables { get; set; }

    [IgnorePropertyParsing(true)]
    public IList<StepVariable> CodeOutputVariables { get; set; }

    public ExecutePython(ScriptEngineHost scriptHost)
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
                context.Workflow.Variables[variable.Name] = variable;
            }
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
