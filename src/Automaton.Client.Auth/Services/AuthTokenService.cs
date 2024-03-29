﻿using Automaton.Client.Auth.Models;
using System.Text;
using System.Text.Json;

namespace Automaton.Client.Auth.Services;

public class AuthTokenService
{
    private const string ApplicationJson = "application/json";

    private readonly ClientAuthConfigurationService configurationService;
    private readonly HttpClient httpClient;

    public AuthTokenService(ClientAuthConfigurationService configurationService, HttpClient httpClient)
    {
        this.configurationService = configurationService;
        this.httpClient = httpClient;
    }

    public async Task<JsonWebToken> GetJsonWebTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var refreshTokenJson = JsonSerializer.Serialize(new { Token = refreshToken });
        var refreshTokenContent = new StringContent(refreshTokenJson, Encoding.UTF8, ApplicationJson);

        var refreshResult = await httpClient.PostAsync(configurationService.RefreshAccessTokenUrl, refreshTokenContent);
        refreshResult.EnsureSuccessStatusCode();

        var refreshContent = await refreshResult.Content.ReadAsStringAsync();
        var jsonWebToken = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (jsonWebToken == null)
        { 
            throw new Exception("Could not retrieve JWT");
        }

        return jsonWebToken;
    }
}
