using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly UserManagerService _userManagerService;
        
        public RemoveUserRoleCommandHandler(IDataContext dataContext,UserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userManagerService.RemoveRole(request.UserId, request.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
