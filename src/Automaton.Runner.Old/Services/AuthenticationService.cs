﻿using Automaton.Client.Auth.Http;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
using Automaton.Client.Auth.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class AuthenticationService
{
    private const string Bearer = "bearer";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions options;
    private readonly AuthStateProvider authStateProvider;
    private readonly ClientAuthConfigurationService configService;
    private readonly IAuthenticationStorage authenticationStorage;

    public AuthenticationService(AutomatonHttpClient automatonHttpClient,
        AuthStateProvider authStateProvider,
        ClientAuthConfigurationService configService,
        IAuthenticationStorage localStorage)
    {
        this.httpClient = automatonHttpClient.Client;
        this.authStateProvider = authStateProvider;
        this.configService = configService;
        this.authenticationStorage = localStorage;
        this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task Login(LoginDetails loginCredentials)
    {
        var loginCredentialsContent = new StringContent(JsonSerializer.Serialize(loginCredentials), Encoding.UTF8, ApplicationJson);
        var result = await httpClient.PostAsync(configService.LoginUserUrl, loginCredentialsContent);
        result.EnsureSuccessStatusCode();

        var jsonToken = await result.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonWebToken>(jsonToken, options);

        await authenticationStorage.SetJsonWebToken(token);

        authStateProvider.NotifyUserAuthentication(token.AccessToken);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
    }

    public async Task Logout()
    {
        await authenticationStorage.DeleteJsonWebToken();

        authStateProvider.NotifyUserLogout();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public bool IsAuthenticated()
    {
        var authenticated = Task.Run(async () => await authStateProvider.IsAuthenticated()).Result;

        return authenticated;
    }
}