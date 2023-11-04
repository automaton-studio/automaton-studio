using AuthServer.Core.Events;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Automaton.Studio.Server.Application.Queries.Handlers
{
    public class RefreshAccessTokenQueryHandler : IRequestHandler<RefreshAccessTokenQuery, JsonWebToken>
    {
        private readonly ApplicationDbContext dataContext;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;
        private readonly UserManagerService userManagerService;
        private readonly ILogger<RefreshAccessTokenQueryHandler> logger;

        public RefreshAccessTokenQueryHandler
        (
            ApplicationDbContext dataContext, 
            IJwtService jwtService, IMediator mediator,
            UserManagerService userManagerService, 
            ILogger<RefreshAccessTokenQueryHandler> logger
        )
        {
            this.dataContext = dataContext;
            this.jwtService = jwtService;
            this.mediator = mediator;
            this.userManagerService = userManagerService;
            this.logger = logger;
        }

        public async Task<JsonWebToken> Handle(RefreshAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await dataContext
                .Set<RefreshToken>()
                .SingleOrDefaultAsync(x => x.Token == request.Token, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token not found.");
            }

            refreshToken.ValidateRefreshToken();

            var user = await userManagerService.GetUserById(refreshToken.UserId);

            if (user == null)
            {
                throw new Exception($"User: '{refreshToken.UserId}' not found.");
            }

            var newRefreshToken = new RefreshToken(user.Id, request.RefreshTokenExpirationDays);

            try
            {
                dataContext.BeginTransaction();
                dataContext.Set<RefreshToken>().Remove(refreshToken);
                await dataContext.Set<RefreshToken>().AddAsync(newRefreshToken, cancellationToken);
                await dataContext.SaveChangesAsync(cancellationToken);
                dataContext.Commit();
            }
            catch
            {
                dataContext.Rollback();
                throw;
            }

            var claims = GetCustomClaimsForUser(user.Id);
            var roles = (await userManagerService.GetRoles(user.Id)).ToImmutableList();
            var jwt = jwtService.GenerateToken(user.Id.ToString("N"), user.UserName, roles, claims);
            jwt.RefreshToken = newRefreshToken.Token;

            await mediator.Publish(new AccessTokenRefreshedEvent(user.Id, jwt.AccessToken, newRefreshToken.Token), cancellationToken);

            logger.Log(LogLevel.Debug, "AccessTokenRefreshed Event Published.");

            return jwt;
        }

        Dictionary<string, string> GetCustomClaimsForUser(Guid userId)
        {
            //Get Custom Claims for User
            return new Dictionary<string, string>();
        }
    }
}