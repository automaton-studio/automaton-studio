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

        public async Task<Unit> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            await _userManager.UpdateProfile(
                userId: command.Id,
                firstName: command.FirstName,
                lastName: command.LastName
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}