using MediatR;
using Automaton.Studio.Server.Models;

namespace Automaton.Studio.Server.Queries;

public class FlowLogsQuery : IRequest<FlowLogsResult>
{
    public Guid FlowId { get; set; }
    public int StartIndex { get; set; }
    public int PageSize { get; set; }
}
