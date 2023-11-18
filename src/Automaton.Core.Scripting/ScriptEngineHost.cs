using IronPython.Hosting;
using Microsoft.Scripting;

namespace Automaton.Core.Scripting;

public class ScriptEngineHost
{
    private const string BuiltinsVariable = "__builtins__";
    private const string ArgvVariable = "argv";

    private readonly ScriptEngineFactory engineFactory;

    public event EventHandler<string>? ScriptTextWritten;

    public ScriptEngineHost(ScriptEngineFactory engineFactory)
    {
        this.engineFactory = engineFactory;

        engineFactory.EventRaisingStreamWriter.NewText += OnTextWritten;
    }

    public IDictionary<string, dynamic> Execute(ScriptResource resource, IDictionary<string, object> inputs)
    {
        var engine = engineFactory.GetEngine(resource.ContentType);

        var scope = engine.CreateScope();
        var source = engine.CreateScriptSourceFromString(resource.Content, SourceCodeKind.Statements);

        engine.GetSysModule().SetVariable(ArgvVariable, inputs.Values.ToList());

        source.Execute(scope);

        scope.RemoveVariable(BuiltinsVariable);

        var items = scope.GetItems();
        var variables = items.ToDictionary(x => x.Key, x => x.Value);

        return variables;
    }

    private void OnTextWritten(object? sender, string e)
    {
        ScriptTextWritten?.Invoke(this, e);
    }
}
