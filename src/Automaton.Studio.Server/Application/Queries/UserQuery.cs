using AuthServer.Core.Dtos;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class UserQuery : IRequest<UserDetails>
    {
        public Guid Id { get; set; }
    }
}