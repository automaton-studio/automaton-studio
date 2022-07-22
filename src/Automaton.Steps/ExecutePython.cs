using Automaton.Core.Models;
using Automaton.Core.Scripting;

namespace Automaton.Steps;

public class ExecutePython : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    public string? Content { get; set; }

    public IList<Variable> InputVariables { get; set; }

    public IList<Variable> OutputVariables { get; set; }

    public ExecutePython(ScriptEngineHost scriptHost)
    {
        this.scriptHost = scriptHost;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var resource = new ScriptResource()
        {
            ContentType = ContentType,
            Content = Content
        };

        var inputVariablesDictionary = InputVariables.ToDictionary(x => x.Name, x => (object)x.Value);

        var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

        foreach (var variable in OutputVariables)
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
