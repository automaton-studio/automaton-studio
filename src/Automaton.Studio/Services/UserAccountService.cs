using Automaton.Studio.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class UserAccountService
{
    private readonly HttpClient httpClient;
    private readonly Client.Auth.Services.ConfigurationService configService;

    public UserAccountService(HttpClient httpClient, 
        Client.Auth.Services.ConfigurationService configService)
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
        var result = await httpClient.PutAsJsonAsync(configService.UpdateUserProfileUrl, userSecurity);

        result.EnsureSuccessStatusCode();
    }
}
