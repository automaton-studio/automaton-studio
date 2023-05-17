using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly UserManagerService _userManager;

        public UpdateUserInfoCommandHandler(IDataContext dataContext, UserManagerService userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            await _userManager.UpdateProfile(
                new Models.UserProfile
                {
                    Id = command.Id.Value,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    UserName = command.UserName,
                    Email = command.Email
                }
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}