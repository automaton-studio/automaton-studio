using MediatR;
using static Automaton.Studio.Server.Models.ApiModels;

namespace Automaton.Studio.Server.Queries;

public class FilterFlowExecutionQuery : IRequest<FlowExecutionResult>
{
    public Guid FlowId { get; set; }
    public int StartIndex { get; set; }
    public int PageSize { get; set; }
}
