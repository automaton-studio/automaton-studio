#nullable disable

using System;

namespace Automaton.Runner.Models;

public class FlowExecution
{
    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public string Status { get; set; }
}

