using Automaton.App.Authentication.Models;
using System.Net.Http.Json;

namespace Automaton.App.Authentication.Services;

public class UserRegisterService
{
    private readonly HttpClient httpClient;
    private readonly Client.Auth.Services.ConfigurationService configService;

    public UserRegisterService(HttpClient httpClient, 
        Client.Auth.Services.ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.configService = configService;
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

        var result = await httpClient.PostAsJsonAsync(configService.RegisterUserUrl, userDetails);

        result.EnsureSuccessStatusCode();
    }
}
