using Common.Authentication;
using MediatR;

namespace Automaton.Studio.Server.Core.Commands
{
    public class RefreshAccessTokenQuery : RefreshAccessTokenCommand, IRequest<JsonWebToken>
    {
        public int RefreshTokenExpirationDays { get; }

        public RefreshAccessTokenQuery(int refreshTokenExpirationDays, string token)
            : base(token)
        {
            RefreshTokenExpirationDays = refreshTokenExpirationDays;
        }
    }
}