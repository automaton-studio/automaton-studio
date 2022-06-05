using MediatR;

namespace Automaton.Studio.Server.Core.Commands
{
    public class ExecuteFlowCommand : IRequest
    {
        public Guid FlowId { get; set; }
        public IEnumerable<Guid> RunnerIds { get; set; }
    }
}