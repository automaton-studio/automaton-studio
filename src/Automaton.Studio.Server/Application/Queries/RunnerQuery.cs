using Automaton.Studio.Server.Models;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class RunnerQuery : IRequest<IEnumerable<Runner>>
    {
        public IEnumerable<Guid> RunnerIds { get; set; } = new List<Guid>();
    }
}
