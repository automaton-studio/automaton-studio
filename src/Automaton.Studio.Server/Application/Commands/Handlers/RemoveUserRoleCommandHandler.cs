using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Commands;
using AuthServer.Core.Services;
using Common.EF;
using MediatR;

namespace AuthServer.Application.Commands.Handlers
{
    public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IUserManagerService _userManagerService;
        
        public RemoveUserRoleCommandHandler(IDataContext dataContext,IUserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task<Unit> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userManagerService.RemoveRole(request.UserId, request.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
