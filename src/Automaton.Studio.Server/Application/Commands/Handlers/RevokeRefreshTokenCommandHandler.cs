using AuthServer.Core.Events;
using Automaton.Studio.Server.Core.Commands;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
    {
        private readonly IDataContext dataContext;
        private readonly IMediator mediator;
        private readonly ILogger<RevokeRefreshTokenCommandHandler> logger;

        public RevokeRefreshTokenCommandHandler(IDataContext dataContext, IMediator mediator,
            ILogger<RevokeRefreshTokenCommandHandler> logger)
        {
            this.dataContext = dataContext;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await dataContext.Set<RefreshToken>()
                .SingleOrDefaultAsync(x => x.Token == request.Token && x.UserId == request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }

            refreshToken.Revoke();
            dataContext.Set<RefreshToken>().Update(refreshToken);
            await dataContext.SaveChangesAsync(cancellationToken);
            
            await mediator.Publish(new RefreshTokenRevokedEvent(refreshToken.Id, refreshToken.UserId, refreshToken.Token), cancellationToken);
            logger.Log(LogLevel.Debug, "RefreshTokenRevoked Event Published.");
        }
    }
}