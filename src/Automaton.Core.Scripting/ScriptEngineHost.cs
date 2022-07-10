using Microsoft.Scripting;

namespace Automaton.Core.Scripting;

public class ScriptEngineHost
{
    private const string BuiltinsVariable = "__builtins__";

    private readonly ScriptEngineFactory engineFactory;

    public ScriptEngineHost(ScriptEngineFactory engineFactory)
    {
        this.engineFactory = engineFactory;
    }

    public IDictionary<string, dynamic> Execute(ScriptResource resource, IDictionary<string, object> inputs)
    {
        var engine = engineFactory.GetEngine(resource.ContentType);
        var scope = engine.CreateScope(inputs);
        var source = engine.CreateScriptSourceFromString(resource.Content, SourceCodeKind.Statements);

        source.Execute(scope);

        scope.RemoveVariable(BuiltinsVariable);

        var items = scope.GetItems();

        var variables = items.ToDictionary(x => x.Key, x => x.Value);

        return variables;
    }
}
