using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using MediatR;
using System.Collections.Immutable;

namespace Automaton.Studio.Server.Application.Queries.Handlers
{
    public class SignInUserQueryHandler : IRequestHandler<SignInUserQuery, JsonWebToken>
    {
        private readonly ApplicationDbContext dataContext;
        private readonly UserManagerService userManagerService;
        private readonly IJwtService jwtService;

        public SignInUserQueryHandler(ApplicationDbContext dataContext, UserManagerService userManagerService, IJwtService jwtService)
        {
            this.dataContext = dataContext;
            this.userManagerService = userManagerService;
            this.jwtService = jwtService;
        }

        public async Task<JsonWebToken> Handle(SignInUserQuery command, CancellationToken cancellationToken)
        {
            var user = await userManagerService.GetUserByEmailOrUserName(command.UserName);

            if (user == null || await userManagerService.ValidatePasswordAsync(user, command.Password) == false)
            {
                throw new Exception("Invalid credentials.");
            }

            var refreshToken = new RefreshToken(user.Id, command.RefreshTokenExpirationDays);
            var roles = (await userManagerService.GetRoles(user.Id)).ToImmutableList();
            var jwt = jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles, GetCustomClaimsForUser(user.Id));

            jwt.RefreshToken = refreshToken.Token;

            await dataContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return jwt;
        }

        private IDictionary<string, string> GetCustomClaimsForUser(Guid userId)
        {
            //Add custom claims here
            return new Dictionary<string, string>();
        }
    }
}