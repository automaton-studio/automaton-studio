using Automaton.Client.Auth.Models;
using Microsoft.Extensions.Configuration;

namespace Automaton.Client.Auth.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AuthenticationConfiguration authConfiguration = new();
    private readonly ApiConfiguration apiConfiguration = new();

    public string LoginUserUrl => authConfiguration.LoginUserUrl;
    public string RefreshAccessTokenUrl => authConfiguration.RefreshAccessTokenUrl;
    public int RefreshTokenExpirationMinutesCheck => authConfiguration.RefreshTokenExpirationMinutesCheck;
    public string BaseUrl => apiConfiguration.BaseUrl;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AuthenticationConfiguration)).Bind(authConfiguration);
        this.configuration.GetSection(nameof(ApiConfiguration)).Bind(apiConfiguration);
    }
}
