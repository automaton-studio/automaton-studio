using Microsoft.AspNetCore.Builder;

namespace Automaton.Common.Auth
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessTokenValidator( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessTokenMiddleware>();
        }
    }
}