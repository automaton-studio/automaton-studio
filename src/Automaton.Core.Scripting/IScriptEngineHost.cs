namespace Automaton.Core.Scripting;

public interface IScriptEngineHost
{
    void Execute(ScriptResource resource, IDictionary<string, object> inputs);
    dynamic EvaluateExpression(string expression, IDictionary<string, object> inputs);
    T EvaluateExpression<T>(string expression, IDictionary<string, object> inputs);
}
