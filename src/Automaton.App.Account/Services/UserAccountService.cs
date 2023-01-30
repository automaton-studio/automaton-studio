using Automaton.App.Account.Config;
using Automaton.App.Account.Models;
using System.Net.Http.Json;

namespace Automaton.App.Account.Services;

public class UserAccountService
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;

    public UserAccountService(HttpClient httpClient, ConfigurationService configService)
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
