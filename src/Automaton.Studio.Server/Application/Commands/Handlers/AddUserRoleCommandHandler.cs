using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand>
    {
        private readonly IDataContext dataContext;
        private readonly UserManagerService userManagerService;

        public AddUserRoleCommandHandler(IDataContext dataContext, UserManagerService userManagerService)
        {
            this.dataContext = dataContext;
            this.userManagerService = userManagerService;
        }
        
        public async Task Handle(AddUserRoleCommand command, CancellationToken cancellationToken)
        {
            await userManagerService.AddRole(command.UserId, command.RoleName);
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}