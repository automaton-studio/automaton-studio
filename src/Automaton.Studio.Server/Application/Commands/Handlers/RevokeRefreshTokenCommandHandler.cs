using System;
using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Commands;
using AuthServer.Core.Events;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthServer.Application.Commands.Handlers
{
    public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly ILogger<RevokeRefreshTokenCommandHandler> _logger;

        public RevokeRefreshTokenCommandHandler(IDataContext dataContext, IMediator mediator,
            ILogger<RevokeRefreshTokenCommandHandler> logger)
        {
            _dataContext = dataContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dataContext.Set<RefreshToken<Guid>>()
                .SingleOrDefaultAsync(x => x.Token == request.Token && x.UserId == request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }

            refreshToken.Revoke();
            _dataContext.Set<RefreshToken<Guid>>().Update(refreshToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(
                new RefreshTokenRevokedEvent(refreshToken.Id, refreshToken.UserId, refreshToken.Token),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "RefreshTokenRevoked Event Published.");
            return Unit.Value;
        }
    }
}