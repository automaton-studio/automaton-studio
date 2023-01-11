using Automaton.Studio.Pages.Register;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

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

    public async Task Register(UserRegisterModel userRegistration)
    {
        var userDetails = new
        {
            UserName = userRegistration.UserName,
            Password = userRegistration.Password,
            Email = userRegistration.UserName
        };

        var result = await httpClient.PostAsJsonAsync(configService.RegisterUserUrl, userDetails);

        result.EnsureSuccessStatusCode();
    }
}
