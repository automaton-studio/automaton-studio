using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly UserManagerService _userManagerService;

        public AddUserRoleCommandHandler(IDataContext dataContext, UserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task Handle(AddUserRoleCommand command, CancellationToken cancellationToken)
        {
            await _userManagerService.AddRole(command.UserId, command.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}