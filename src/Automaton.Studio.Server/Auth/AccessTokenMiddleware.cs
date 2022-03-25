using System.Net;

namespace Automaton.Studio.Server.Auth
{
    public class AccessTokenMiddleware : IMiddleware
    {
        private readonly IAccessTokenManagerService _accessTokenManagerService;

        public AccessTokenMiddleware(IAccessTokenManagerService accessTokenManagerService)
        {
            _accessTokenManagerService = accessTokenManagerService;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _accessTokenManagerService.CurrentAccessTokenIsActive())
            {
                await next(context);
                
                return;
            }
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}