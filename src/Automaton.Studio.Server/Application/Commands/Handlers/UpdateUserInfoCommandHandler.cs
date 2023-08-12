using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly ApplicationDbContext dataContext;
        private readonly UserManagerService userManager;

        public UpdateUserInfoCommandHandler(ApplicationDbContext dataContext, UserManagerService userManager)
        {
            this.dataContext = dataContext;
            this.userManager = userManager;
        }

        public async Task Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            await userManager.UpdateProfile(
                new Models.UserProfile
                {
                    Id = command.Id.Value,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    UserName = command.UserName,
                    Email = command.Email
                }
            );

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}