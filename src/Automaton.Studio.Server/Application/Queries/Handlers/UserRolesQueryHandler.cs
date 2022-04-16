using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Queries;
using AuthServer.Core.Services;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class UserRolesQueryHandler : IRequestHandler<UserRolesQuery, IEnumerable<string>>
    {
        private readonly IUserManagerService _userManagerService;

        public UserRolesQueryHandler(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public Task<IEnumerable<string>> Handle(UserRolesQuery request, CancellationToken cancellationToken)
        {
            return _userManagerService.GetRoles(request.UserId);
        }
    }
}