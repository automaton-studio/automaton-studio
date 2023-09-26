#nullable disable

namespace Automaton.Runner.Models;

public class FlowExecution
{
    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public string Status { get; set; }
    public string Application { get; set; }
}

