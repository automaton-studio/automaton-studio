using Microsoft.Scripting.Hosting;

namespace Automaton.Core.Scripting;

class ScriptEngineFactory : IScriptEngineFactory
{
    private Dictionary<string, ScriptEngine> _engines = new Dictionary<string, ScriptEngine>()
    {
        [@"text/x-python"] = IronPython.Hosting.Python.CreateEngine(),
        [string.Empty] = IronPython.Hosting.Python.CreateEngine()
    };

    public ScriptEngine GetEngine(string contentType) => _engines[contentType];
    public ScriptEngine GetExpressionEngine() => _engines[string.Empty];
}
