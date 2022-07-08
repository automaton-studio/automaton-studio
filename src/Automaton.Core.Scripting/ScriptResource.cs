namespace Automaton.Core.Scripting;

public class ScriptResource
{
    public string? Name { get; set; }

    public string? ContentType { get; set; }

    public string? Content { get; set; }

    public byte[]? CompiledContent { get; set; }
}
