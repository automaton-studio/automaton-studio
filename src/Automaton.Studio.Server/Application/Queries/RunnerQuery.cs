using Automaton.Studio.Server.Models;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class RunnerQuery : IRequest<IEnumerable<Runner>>
    {
    }
}
