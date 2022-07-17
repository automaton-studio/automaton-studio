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

    public override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var resource = new ScriptResource()
        {
            ContentType = ContentType,
            Content = Content
        };

        var inputVariablesDictionary = InputVariables.ToDictionary(x => x.Name, x => (object)x.Value);

        var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

        UpdateWorkflowVariables(context.Workflow, scriptVariables);

        return Task.FromResult(ExecutionResult.Next());
    }

    private void UpdateWorkflowVariables(Workflow workflow, IDictionary<string, dynamic> scriptVariables)
    {
        foreach (var variable in OutputVariables)
        {
            if (scriptVariables.ContainsKey(variable.Name))
            {
                variable.Value = scriptVariables[variable.Name];
                workflow.Variables[variable.Name] = variable.Value;
            }
        }
    }
}
