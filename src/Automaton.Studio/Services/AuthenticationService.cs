using Automaton.Studio.AuthProviders;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string Bearer = "bearer";
        private const string ApplicationJson = "application/json";

        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ConfigService _configService;
        private readonly IStorageService _localStorage;

        public AuthenticationService(HttpClient client, 
            AuthenticationStateProvider authStateProvider,
            ConfigService configService,
            IStorageService localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _configService = configService;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginCredentials loginCredentials)
        {
            var content = JsonSerializer.Serialize(loginCredentials);
            var bodyContent = new StringContent(content, Encoding.UTF8, ApplicationJson);

            var result = await _client.PostAsync(_configService.LoginUserUrl, bodyContent);
            var jsonToken = await result.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JsonWebToken>(jsonToken, _options);

            if (!result.IsSuccessStatusCode)
                return false;

            await _localStorage.SetAuthAndRefreshTokens(token);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(token.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
            
            return true;
        }

        public async Task Logout()
        {
            await _localStorage.DeleteAuthAndRefreshTokens();

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
