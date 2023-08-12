using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand>
    {
        private readonly ApplicationDbContext dataContext;
        private readonly UserManagerService userManagerService;
        
        public RemoveUserRoleCommandHandler(ApplicationDbContext dataContext,UserManagerService userManagerService)
        {
            this.dataContext = dataContext;
            this.userManagerService = userManagerService;
        }
        
        public async Task Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            await userManagerService.RemoveRole(request.UserId, request.RoleName);
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
