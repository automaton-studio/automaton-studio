using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Parsers;
using Automaton.Client.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Automaton.Client.Auth.Providers
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private const string Bearer = "bearer";
        private const string ClaimExp = "exp";
        private const string ClaimJwtAuthType = "jwtAuthType";

        private readonly HttpClient httpClient;
        private readonly IAuthenticationStorage localStorage;
        private readonly RefreshTokenService refreshTokenService;
        private readonly ConfigurationService configService;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(HttpClient httpClient,
            IAuthenticationStorage localStorage, 
            RefreshTokenService refreshTokenService,
            ConfigurationService configService)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.refreshTokenService = refreshTokenService;
            this.configService = configService;
            this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var authToken = await GetAuthToken();

                if (string.IsNullOrWhiteSpace(authToken))
                {
                    return anonymous;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, authToken);

                var claims = JsonWebTokenParser.ParseClaimsFromJwt(authToken);
                var claimsIdentity = new ClaimsIdentity(claims, ClaimJwtAuthType);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authenticationState = new AuthenticationState(claimsPrincipal);

                return authenticationState;

            }
            catch
            {
                // The following error is triggered first time the application is accessed:

                // "JavaScript interop calls cannot be issued at this time.
                // This is because the component is being statically rendered.
                // When prerendering is enabled, JavaScript interop calls can only
                // be performed during the OnAfterRenderAsync lifecycle method."

                // No matter what other exceptions may be triggered, always return anonymous.

                return anonymous;
            }
        }

        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JsonWebTokenParser.ParseClaimsFromJwt(token), ClaimJwtAuthType));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonymous);

            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<string> GetAuthToken()
        {
            var authToken = await localStorage.GetAuthToken()??string.Empty;
            var claims = JsonWebTokenParser.ParseClaimsFromJwt(authToken);
            var claimsIdentity = new ClaimsIdentity(claims, ClaimJwtAuthType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authState = new AuthenticationState(claimsPrincipal);
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals(ClaimExp)).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            var token = diff.TotalMinutes <= configService.RefreshTokenExpirationMinutesCheck ? 
                await refreshTokenService.RefreshToken() : 
                authToken;

            return token;
        }
    }
}
