using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using Automaton.Studio.Services.Interfaces;

namespace Automaton.Studio.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, IRefreshTokenService refreshTokenService)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.refreshTokenService = refreshTokenService;
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

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);

                var claims = JwtParser.ParseClaimsFromJwt(authToken);
                var claimsIdentity = new ClaimsIdentity(claims, "jwtAuthType");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authenticationState = new AuthenticationState(claimsPrincipal);

                return authenticationState;

            }
            catch
            {
                // 1. The following error is triggered first time the application is accessed:

                // "JavaScript interop calls cannot be issued at this time.
                // This is because the component is being statically rendered.
                // When prerendering is enabled, JavaScript interop calls can only
                // be performed during the OnAfterRenderAsync lifecycle method."

                // 2. If any other exception is triggered, return anonymous

                return anonymous;
            }
        }

        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
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
            var authToken = await localStorage.GetItemAsync<string>("authToken");
            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authToken), "jwtAuthType")));
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            var token = diff.TotalMinutes <= 2 ? await refreshTokenService.RefreshToken() : authToken;

            return token;
        }
    }
}
