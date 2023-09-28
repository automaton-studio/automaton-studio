using MediatR;
using Automaton.Studio.Server.Models;

namespace Automaton.Studio.Server.Queries;

public class FlowExecutionQuery : IRequest<FlowExecutionResult>
{
    public Guid FlowId { get; set; }
    public int StartIndex { get; set; }
    public int PageSize { get; set; }
}
