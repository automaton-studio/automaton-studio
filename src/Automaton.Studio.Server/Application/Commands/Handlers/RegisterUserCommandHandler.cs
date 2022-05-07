using AuthServer.Core.Domains;
using AuthServer.Core.Events;
using AutoMapper;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using Common.EF;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly UserManagerService _userManagerService;

        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IDataContext dataContext, IMapper mapper,
            ILogger<RegisterUserCommandHandler> logger,
            IMediator mediator, UserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _userManagerService = userManagerService;
        }

        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            Guid id = command.Id != Guid.Empty ? command.Id : Guid.NewGuid();
            await _userManagerService.CreateUser(new ApplicationUser()
            {
                Id = id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                Email = command.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            }, command.Password);

            await _dataContext.SaveChangesAsync(cancellationToken);

            var userRegisteredEvent = _mapper.Map<UserRegisteredEvent>(command);
            await _mediator.Publish(userRegisteredEvent, cancellationToken);
            _logger.Log(LogLevel.Debug, "UserRegistered Event Published.");

            return Unit.Value;
        }
    }
}