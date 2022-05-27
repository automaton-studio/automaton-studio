using Automaton.Client.Auth.Providers;
using System.Net.Http.Headers;

namespace Automaton.Client.Auth.Handlers
{
    public class TokenAuthHeaderHandler : DelegatingHandler
    {
        private readonly AuthStateProvider _tokenProvider;

        public TokenAuthHeaderHandler(AuthStateProvider tokenProvider) => _tokenProvider = tokenProvider;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetAccessTokenAsync());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
