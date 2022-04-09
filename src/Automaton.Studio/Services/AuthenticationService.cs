using Automaton.Studio.AuthProviders;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Blazored.LocalStorage;
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

            var result = await _client.PostAsync(_configService.TokenUrl, bodyContent);
            var authContent = await result.Content.ReadAsStringAsync();
            var authenticationResult = JsonSerializer.Deserialize<AuthenticationResult>(authContent, _options);

            if (!result.IsSuccessStatusCode)
                return false;

            await _localStorage.SetItemAsync("authToken", authenticationResult.AccessToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(authenticationResult.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResult.AccessToken);
            
            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
