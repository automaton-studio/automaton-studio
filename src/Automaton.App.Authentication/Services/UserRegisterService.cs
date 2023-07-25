using Automaton.App.Authentication.Models;
using Automaton.Client.Auth.Services;
using System.Net.Http.Json;

namespace Automaton.App.Authentication.Services;

public class UserRegisterService
{
    private readonly HttpClient httpClient;
    private readonly ClientAuthConfigurationService clientAuthConfigService;

    public UserRegisterService(HttpClient httpClient, ClientAuthConfigurationService clientAuthConfigService)
    {
        this.httpClient = httpClient;
        this.clientAuthConfigService = clientAuthConfigService;
    }

    public async Task Register(UserRegister userRegistration)
    {
        var userDetails = new
        {
            userRegistration.UserName,
            userRegistration.FirstName,
            userRegistration.LastName,
            userRegistration.Email,
            userRegistration.Password,
        };

        var result = await httpClient.PostAsJsonAsync(clientAuthConfigService.RegisterUserUrl, userDetails);

        result.EnsureSuccessStatusCode();
    }
}
