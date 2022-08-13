using Automaton.Client.Auth.Providers;
using System.Net.Http.Headers;

namespace Automaton.Client.Auth.Handlers;

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
        var accessToken = await authStateProvider.GetAccessTokenAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, accessToken);
       
        return await base.SendAsync(request, cancellationToken);
    }
}
