using Automaton.Core.Enums;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Models;

public class RunnerFlowResult
{
    public Guid FlowId { get; set; }
    public Guid RunnerId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; }
}
