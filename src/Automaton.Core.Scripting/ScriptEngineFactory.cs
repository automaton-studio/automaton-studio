using Microsoft.Scripting.Hosting;

namespace Automaton.Core.Scripting;

public class ScriptEngineFactory
{
    private readonly Dictionary<string, ScriptEngine> engines = new()
    {
        [@"text/x-python"] = IronPython.Hosting.Python.CreateEngine(),
    };

    public ScriptEngine GetEngine(string contentType) => engines[contentType];
}
