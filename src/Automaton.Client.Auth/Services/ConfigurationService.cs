using Automaton.Client.Auth.Models;
using Microsoft.Extensions.Configuration;

namespace Automaton.Client.Auth.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AuthenticationConfig authConfiguration = new();
    private readonly ApiConfig apiConfiguration = new();

    public string RegisterUserUrl => authConfiguration.RegisterUserUrl;
    public string UpdateUserProfileUrl => authConfiguration.UpdateUserProfileUrl;
    public string UpdateUserPasswordUrl => authConfiguration.UpdateUserPasswordUrl;
    public string GetUserProfileUrl => authConfiguration.GetUserProfileUrl;
    public string LoginUserUrl => authConfiguration.LoginUserUrl;
    public string RefreshAccessTokenUrl => authConfiguration.RefreshAccessTokenUrl;
    public int RefreshTokenExpirationMinutesCheck => authConfiguration.RefreshTokenExpirationMinutesCheck;
    public string BaseUrl => apiConfiguration.BaseUrl;
    public string LogsUrl => $"{apiConfiguration.BaseUrl}{apiConfiguration.LogsUrl}";

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authConfiguration);
        this.configuration.GetSection(nameof(ApiConfig)).Bind(apiConfiguration);
    }
}
