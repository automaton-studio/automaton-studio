namespace Automaton.Studio.Server.Auth
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessTokenValidator( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessTokenMiddleware>();
        }
    }
}