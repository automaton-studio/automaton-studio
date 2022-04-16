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
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, IRefreshTokenService refreshTokenService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _refreshTokenService = refreshTokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var authToken = await GetAuthToken();

                if (string.IsNullOrWhiteSpace(authToken))
                {
                    return _anonymous;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authToken), "jwtAuthType")));

            }
            catch (Exception ex)
            {
                // JavaScript interop calls cannot be issued at this time.
                // This is because the component is being statically rendered.
                // When prerendering is enabled, JavaScript interop calls can only
                // be performed during the OnAfterRenderAsync lifecycle method.

                return _anonymous;
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
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<string> GetAuthToken()
        {
            var authToken = await _localStorage.GetItemAsync<string>("authToken");
            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authToken), "jwtAuthType")));
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            var token = diff.TotalMinutes <= 2 ? 
                await _refreshTokenService.RefreshToken() :
                authToken;

            return token;
        }
    }
}
