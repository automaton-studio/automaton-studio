using Microsoft.Scripting.Hosting;

namespace Automaton.Core.Scripting;

public interface IScriptEngineFactory
{
    ScriptEngine GetEngine(string contentType);
    ScriptEngine GetExpressionEngine();
}
