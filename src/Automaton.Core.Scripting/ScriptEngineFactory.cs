using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Text;

namespace Automaton.Core.Scripting;

public class ScriptEngineFactory
{
    private const string PythonScrypt = @"text/x-python";

    private readonly Dictionary<string, ScriptRuntime> runtimes;
    private readonly Dictionary<string, ScriptEngine> engines;

    public EventRaisingStreamWriter EventRaisingStreamWriter { get; set; }

    public ScriptEngineFactory()
    {
        EventRaisingStreamWriter = new EventRaisingStreamWriter();

        runtimes = new()
        {
            [PythonScrypt] = Python.CreateRuntime(),
        };

        // Changes to the Shared IO are not picked up 
        // Workaround is to call SetOutput before creating of the engine
        // https://github.com/IronLanguages/ironpython3/issues/961
        runtimes[PythonScrypt].IO.SetOutput(EventRaisingStreamWriter, Encoding.ASCII);

        engines = new()
        {
            [PythonScrypt] = Python.GetEngine(runtimes[PythonScrypt])
        };
    }

    public ScriptEngine GetEngine(string contentType) => engines[contentType];

    public ScriptRuntime GetRuntime(string contentType) => runtimes[contentType];
}
