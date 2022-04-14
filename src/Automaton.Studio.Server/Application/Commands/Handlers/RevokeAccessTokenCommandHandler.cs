using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Commands;
using AuthServer.Core.Events;
using Common.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthServer.Application.Commands.Handlers
{
    public class RevokeAccessTokenCommandHandler : IRequestHandler<RevokeAccessTokenCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RevokeAccessTokenCommandHandler> _logger;
        private readonly IAccessTokenManagerService _accessTokenManagerService;

        public RevokeAccessTokenCommandHandler(IMediator mediator, ILogger<RevokeAccessTokenCommandHandler> logger,
            IAccessTokenManagerService accessTokenManagerService)
        {
            _mediator = mediator;
            _logger = logger;
            _accessTokenManagerService = accessTokenManagerService;
        }

        public async Task<Unit> Handle(RevokeAccessTokenCommand request, CancellationToken cancellationToken)
        {
            await _accessTokenManagerService.DeactivateAccessTokenAsync(request.AccessToken);

            await _mediator.Publish(new AccessTokenRevokedEvent(request.UserId, request.AccessToken),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "AccessTokenRevoked Event Published.");

            return Unit.Value;
        }
    }
}