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
    public class AuthenticationService
    {
        private const string Bearer = "bearer";
        private const string ApplicationJson = "application/json";

        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ConfigService configService;
        private readonly LocalStorageService localStorage;

        public AuthenticationService(HttpClient client, 
            AuthenticationStateProvider authStateProvider,
            ConfigService configService,
            LocalStorageService localStorage)
        {
            this.client = client;
            this.authStateProvider = authStateProvider;
            this.configService = configService;
            this.localStorage = localStorage;
            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<bool> Login(LoginCredentials loginCredentials)
        {
            var content = JsonSerializer.Serialize(loginCredentials);
            var bodyContent = new StringContent(content, Encoding.UTF8, ApplicationJson);

            var result = await client.PostAsync(configService.LoginUserUrl, bodyContent);

            if (!result.IsSuccessStatusCode)
                return false;

            var jsonToken = await result.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JsonWebToken>(jsonToken, options);

            await localStorage.SetAuthAndRefreshTokens(token);

            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(token.AccessToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
            
            return true;
        }

        public async Task Logout()
        {
            await localStorage.DeleteAuthAndRefreshTokens();

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
