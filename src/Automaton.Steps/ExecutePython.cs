using Automaton.Core.Models;
using Automaton.Core.Scripting;

namespace Automaton.Steps;

public class ExecutePython : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly IScriptEngineHost _scriptHost;

    public string? Content { get; set; }

    public ExecutePython(IScriptEngineHost scriptHost)
    {
        _scriptHost = scriptHost;
    }

    public override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var resource = new ScriptResource()
        {
            ContentType = ContentType,
            Content = Content
        };

        var variables = Inputs as IDictionary<string, object>;

        _scriptHost.Execute(resource, variables);

        return Task.FromResult(ExecutionResult.Next());
    }
}
