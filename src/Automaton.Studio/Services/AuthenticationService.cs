﻿using Automaton.Client.Auth.Http;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
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

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly Client.Auth.Services.ConfigurationService configService;
        private readonly IAuthenticationStorage authenticationStorage;

        public AuthenticationService(HttpClient automatonHttpClient, 
            AuthenticationStateProvider authStateProvider,
            Client.Auth.Services.ConfigurationService configService,
            IAuthenticationStorage localStorage)
        {
            this.httpClient = automatonHttpClient;
            this.authStateProvider = authStateProvider;
            this.configService = configService;
            this.authenticationStorage = localStorage;
            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task Login(LoginDetails loginCredentials)
        {
            var content = JsonSerializer.Serialize(loginCredentials);
            var bodyContent = new StringContent(content, Encoding.UTF8, ApplicationJson);

            var result = await httpClient.PostAsync(configService.LoginUserUrl, bodyContent);

            result.EnsureSuccessStatusCode();

            var jsonToken = await result.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JsonWebToken>(jsonToken, options);

            await authenticationStorage.SetJsonWebToken(token);

            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(token.AccessToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
        }

        public async Task Logout()
        {
            await authenticationStorage.DeleteJsonWebToken();

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> InitLoggedInAuthorization()
        {
            var jsonWebToken = await authenticationStorage.GetJsonWebToken();
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, jsonWebToken.AccessToken);
                
            return jsonWebToken.IsValid();
        }
    }
}