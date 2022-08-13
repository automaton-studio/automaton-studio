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
    private readonly AuthenticationState anonymousState;
    private readonly JsonSerializerOptions options;
    private readonly AuthTokenService authTokenService;  

    public AuthStateProvider(HttpClient httpClient,
        IAuthenticationStorage localStorage,
        AuthTokenService authTokenService,
        ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(configService.BaseUrl);
        this.localStorage = localStorage;
        this.configService = configService;
        this.authTokenService = authTokenService;
        this.anonymousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var authToken = await GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(authToken))
            {
                return anonymousState;
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

            return anonymousState;
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
        var authState = Task.FromResult(anonymousState);

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<string> GetAccessTokenAsync()
    {
        try
        {
            var jsonWebToken = await localStorage.GetJsonWebToken();

            if (!IsAccessTokenValid(jsonWebToken.AccessToken))
            {
                jsonWebToken = await UpdateJsonWebTokenAsync();
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, jsonWebToken.AccessToken);

            return jsonWebToken.AccessToken;
        }
        catch (Exception)
        {
            return string.Empty;        
        }     
    }

    private async Task<JsonWebToken> UpdateJsonWebTokenAsync()
    {
        var refreshToken = await localStorage.GetRefreshToken();

        var jsonWebToken = await authTokenService.GetJsonWebTokenAsync(refreshToken);

        await localStorage.SetJsonWebToken(jsonWebToken);

        return jsonWebToken;
    }

    private bool IsAccessTokenValid(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return false;

        var claims = JsonWebTokenParser.ParseClaimsFromJwt(accessToken);
        var claimsIdentity = new ClaimsIdentity(claims, ClaimJwtAuthType);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authState = new AuthenticationState(claimsPrincipal);

        var expirationClaim = authState.User.FindFirst(c => c.Type.Equals(ClaimExp));
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expirationClaim.Value));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;

        var tokenValid = diff.TotalMinutes > configService.RefreshTokenExpirationMinutesCheck;

        return tokenValid;
    }
}
