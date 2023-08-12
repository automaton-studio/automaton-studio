using AuthServer.Core.Events;
using Automaton.Studio.Server.Core.Commands;
using Common.Authentication;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RevokeAccessTokenCommandHandler : IRequestHandler<RevokeAccessTokenCommand>
    {
        private readonly IMediator mediator;
        private readonly ILogger<RevokeAccessTokenCommandHandler> logger;
        private readonly IAccessTokenManagerService accessTokenManagerService;

        public RevokeAccessTokenCommandHandler(IMediator mediator, ILogger<RevokeAccessTokenCommandHandler> logger,
            IAccessTokenManagerService accessTokenManagerService)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.accessTokenManagerService = accessTokenManagerService;
        }

        public async Task Handle(RevokeAccessTokenCommand request, CancellationToken cancellationToken)
        {
            await accessTokenManagerService.DeactivateAccessTokenAsync(request.AccessToken);

            await mediator.Publish(new AccessTokenRevokedEvent(request.UserId, request.AccessToken), cancellationToken);

            logger.Log(LogLevel.Debug, "AccessTokenRevoked Event Published.");
        }
    }
}