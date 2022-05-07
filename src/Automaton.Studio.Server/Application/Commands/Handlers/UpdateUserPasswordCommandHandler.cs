using AuthServer.Core.Events;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
    {
        private readonly UserManagerService _userManagerService;
        private readonly IDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

        public UpdateUserPasswordCommandHandler(UserManagerService userManagerService, IDataContext dataContext,
            IMediator mediator, ILogger<UpdateUserPasswordCommandHandler> logger)
        {
            _userManagerService = userManagerService;
            _dataContext = dataContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userManagerService.UpdatePassword(request.UserId, request.OldPassword, request.NewPassword);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new PasswordChangedEvent(request.UserId, request.OldPassword, request.NewPassword),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "PasswordChanged Event Published.");
            return Unit.Value;
        }
    }
}