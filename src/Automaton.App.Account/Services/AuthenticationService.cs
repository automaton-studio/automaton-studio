using Automaton.App.Account.Config;
using Automaton.App.Account.Models;

namespace Automaton.App.Account.Services;

public class AuthenticationService
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;

    public AuthenticationService(HttpClient httpClient, ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.configService = configService;
    }

    public async Task<HttpResponseMessage> Login(LoginDetails loginCredentials)
    {
        var result = await httpClient.PostAsJsonAsync(configService.LoginUserUrl, loginCredentials);

        return result;
    }
}
