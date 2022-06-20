using Automaton.Studio.Server.Models;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class FilterUserQuery : IRequest<IEnumerable<UserDetails>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
