using Common.Authentication;
using MediatR;

namespace Automaton.Studio.Server.Core.Commands;

public class SignInUserQuery : SignInUserCommand, IRequest<JsonWebToken>
{
    public int RefreshTokenExpirationDays { get; }

    public SignInUserQuery(int refreshTokenExpirationDays, string userName, string password)
        : base(userName, password)
    {
        RefreshTokenExpirationDays = refreshTokenExpirationDays;
    }
}