using Automaton.App.Account.Models;
using Automaton.Client.Auth.Services;
using System.Net.Http.Json;

namespace Automaton.App.Account.Services;

public class UserAccountService
{
    private readonly HttpClient httpClient;
    private readonly ClientAuthConfigurationService configService;

    public UserAccountService(HttpClient httpClient, ClientAuthConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.configService = configService;
    }

    public async Task<UserProfile> GetUserProfile()
    {
        var result = await httpClient.GetAsync(configService.GetUserProfileUrl);

        result.EnsureSuccessStatusCode();

        var userProfile = await result.Content.ReadAsAsync<UserProfile>();

        return userProfile;
    }

    public async Task UpdateUserProfile(UserProfile userProfile)
    {
        var result = await httpClient.PutAsJsonAsync(configService.UpdateUserProfileUrl, userProfile);

        result.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserPassword(UserPassword userSecurity)
    {
        var result = await httpClient.PutAsJsonAsync(configService.UpdateUserPasswordUrl, userSecurity);

        result.EnsureSuccessStatusCode();
    }
} 
