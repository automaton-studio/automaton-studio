using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand>
    {
        private readonly ApplicationDbContext dataContext;
        private readonly UserManagerService userManagerService;

        public AddUserRoleCommandHandler(ApplicationDbContext dataContext, UserManagerService userManagerService)
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