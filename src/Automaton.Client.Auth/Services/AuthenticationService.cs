using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Automaton.Client.Auth.Services
{
    public class AuthenticationService
    {
        private const string Bearer = "bearer";
        private const string ApplicationJson = "application/json";

        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ConfigurationService configService;
        private readonly IStorageService localStorage;

        public AuthenticationService(HttpClient client, 
            AuthenticationStateProvider authStateProvider,
            ConfigurationService configService,
            IStorageService localStorage)
        {
            this.client = client;
            this.authStateProvider = authStateProvider;
            this.configService = configService;
            this.localStorage = localStorage;
            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task Login(LoginDetails loginCredentials)
        {
            var content = JsonSerializer.Serialize(loginCredentials);
            var bodyContent = new StringContent(content, Encoding.UTF8, ApplicationJson);

            var result = await client.PostAsync(configService.LoginUserUrl, bodyContent);

            result.EnsureSuccessStatusCode();

            var jsonToken = await result.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JsonWebToken>(jsonToken, options);

            await localStorage.SetJsonWebToken(token);

            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(token.AccessToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
        }

        public async Task Logout()
        {
            await localStorage.DeleteJsonWebToken();

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
