using Automaton.Studio.AuthProviders;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ConfigService _configService;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient client, 
            AuthenticationStateProvider authStateProvider,
            ConfigService configService,
            ILocalStorageService localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _configService = configService;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginModel userCredentials)
        {
            var content = JsonSerializer.Serialize(userCredentials);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(_configService.LoginUserUrl, bodyContent);
            var authContent = await result.Content.ReadAsStringAsync();
            var authenticationResult = JsonSerializer.Deserialize<JsonWebToken>(authContent, _options);

            if (!result.IsSuccessStatusCode)
                return false;

            await _localStorage.SetItemAsync("authToken", authenticationResult.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", authenticationResult.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(authenticationResult.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResult.AccessToken);
            
            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var tokenDto = JsonSerializer.Serialize(new JsonWebToken { AccessToken = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
            var refreshResult = await _client.PostAsync("token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, _options);
            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");
            await _localStorage.SetItemAsync("authToken", result.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            return result.AccessToken;
        }
    }
}
