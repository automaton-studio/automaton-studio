using MediatR;

namespace Automaton.Studio.Server.Models
{
    public class ExecuteFlowDetails : IRequest
    {
        public Guid FlowId { get; set; }
        public IEnumerable<Guid> RunnerIds { get; set; }
    }
}