using Automaton.Studio.Server.Models;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class UserQuery : IRequest<UserDetails>
    {
        public Guid Id { get; set; }
    }
}