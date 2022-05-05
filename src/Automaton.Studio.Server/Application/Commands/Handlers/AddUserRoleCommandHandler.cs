using Automaton.Studio.Server.Core.Commands;
using Common.EF;
using MediatR;
using Automaton.Studio.Server.Services.Interfaces;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IUserManagerService _userManagerService;

        public AddUserRoleCommandHandler(IDataContext dataContext, IUserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task<Unit> Handle(AddUserRoleCommand command, CancellationToken cancellationToken)
        {
            await _userManagerService.AddRole(command.UserId, command.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return  Unit.Value;
        }
    }
}