using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Commands;
using AuthServer.Core.Services;
using Common.EF;
using MediatR;

namespace AuthServer.Application.Commands.Handlers
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IUserManagerService _userManager;

        public UpdateUserInfoCommandHandler(IDataContext dataContext, IUserManagerService userManager)
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