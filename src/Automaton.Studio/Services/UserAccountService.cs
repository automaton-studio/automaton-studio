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

    public async Task UpdateUserProfile(UserUpdate userUpdate)
    {
        var result = await httpClient.PutAsJsonAsync(configService.UpdateUserProfileUrl, userUpdate);

        result.EnsureSuccessStatusCode();
    }
}
