using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Automaton.Common.Auth
{
    public interface IJwtService
    {
        JsonWebToken GenerateToken(string userId, string userName, IDictionary<string, string> claims = null);

        JsonWebTokenPayload ValidateAndGetTokenPayload(string token, TokenValidationParameters tokenValidationParameters = null);

        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters = null);
    }
}