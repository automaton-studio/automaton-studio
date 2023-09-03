using Automaton.App.Authentication.Config;
using Automaton.App.Authentication.Events;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Automaton.App.Authentication.Services;

public class AuthenticationService
{
    private const string Bearer = "bearer";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authStateProvider;
    private readonly ConfigurationService configService;
    private readonly IAuthenticationStorage authenticationStorage;
    private readonly IMediator mediator;

    public AuthenticationService(HttpClient automatonHttpClient,
        AuthenticationStateProvider authStateProvider,
        ConfigurationService configService,
        IAuthenticationStorage localStorage,
        IMediator mediator)
    {
        this.httpClient = automatonHttpClient;
        this.authStateProvider = authStateProvider;
        this.configService = configService;
        this.authenticationStorage = localStorage;
        this.mediator = mediator;
    }

    public async Task Login(LoginDetails loginCredentials)
    {
        var result = await httpClient.PostAsJsonAsync(configService.LoginUserUrl, loginCredentials);

        result.EnsureSuccessStatusCode();

        var token = await result.Content.ReadAsAsync<JsonWebToken>();
        await authenticationStorage.SetJsonWebToken(token);

        ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(token.AccessToken);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);

        await mediator.Publish(new UserLoginNotification());
    }

    public async Task Logout()
    {
        await authenticationStorage.DeleteJsonWebToken();

        ((AuthStateProvider)authStateProvider).NotifyUserLogout();
        httpClient.DefaultRequestHeaders.Authorization = null;

        await mediator.Publish(new UserLogoutNotification());
    }
}
