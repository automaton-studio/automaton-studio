using Automaton.Client.Auth.Providers;
using System.Net.Http.Headers;

namespace Automaton.Client.Auth.Handlers
{
    public class TokenAuthHeaderHandler : DelegatingHandler
    {
        private const string Bearer = "bearer";

        private readonly AuthStateProvider authStateProvider;

        public TokenAuthHeaderHandler(AuthStateProvider authStateProvider)
        {
            this.authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, await authStateProvider.GetAccessTokenAsync());
           
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
