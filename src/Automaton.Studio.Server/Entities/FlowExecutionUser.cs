#nullable disable

namespace Automaton.Studio.Server.Entities;

public class FlowExecutionUser
{
    public Guid UserId { get; set; }
    public Guid FlowExecutionId { get; set; }
}
