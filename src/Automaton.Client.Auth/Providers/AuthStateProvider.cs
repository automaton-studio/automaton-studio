using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Parsers;
using Automaton.Client.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Automaton.Client.Auth.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private const string Bearer = "bearer";
    private const string ClaimExp = "exp";
    private const string ClaimJwtAuthType = "jwtAuthType";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient httpClient;
    private readonly IAuthenticationStorage localStorage;
    private readonly ConfigurationService configService;
    private readonly AuthenticationState anonymous;
    private readonly JsonSerializerOptions options;

    public AuthStateProvider(HttpClient httpClient,
        IAuthenticationStorage localStorage, 
        ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(configService.BaseUrl);
        this.localStorage = localStorage;
        this.configService = configService;
        this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var authToken = await GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(authToken))
            {
                return anonymous;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, authToken);

            var claims = JsonWebTokenParser.ParseClaimsFromJwt(authToken);
            var claimsIdentity = new ClaimsIdentity(claims, ClaimJwtAuthType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationState = new AuthenticationState(claimsPrincipal);

            return authenticationState;

        }
        catch
        {
            // The following error is triggered first time the application is accessed:

            // "JavaScript interop calls cannot be issued at this time.
            // This is because the component is being statically rendered.
            // When prerendering is enabled, JavaScript interop calls can only
            // be performed during the OnAfterRenderAsync lifecycle method."

            // No matter what other exceptions may be triggered, always return anonymous.

            return anonymous;
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JsonWebTokenParser.ParseClaimsFromJwt(token), ClaimJwtAuthType));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(anonymous);

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var authToken = await localStorage.GetAuthToken();

        if(string.IsNullOrWhiteSpace(authToken))
            return string.Empty;

        var claims = JsonWebTokenParser.ParseClaimsFromJwt(authToken);
        var claimsIdentity = new ClaimsIdentity(claims, ClaimJwtAuthType);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authState = new AuthenticationState(claimsPrincipal);
        var user = authState.User;

        var exp = user.FindFirst(c => c.Type.Equals(ClaimExp)).Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;

        var token = diff.TotalMinutes <= configService.RefreshTokenExpirationMinutesCheck ?
            await RefreshAccessTokenAsync() :
            authToken;

        return token;
    }

    public async Task<string> RefreshAccessTokenAsync()
    {
        var refreshToken = await localStorage.GetRefreshToken();
        var jsonToken = JsonSerializer.Serialize(new { Token = refreshToken });
        var bodyContent = new StringContent(jsonToken, Encoding.UTF8, ApplicationJson);
        var refreshResult = await httpClient.PostAsync(configService.RefreshAccessTokenUrl, bodyContent);
        refreshResult.EnsureSuccessStatusCode();
        var refreshContent = await refreshResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, options);

        await localStorage.SetJsonWebToken(result);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, result.AccessToken);

        return result.AccessToken;
    }
}
