using Microsoft.Extensions.Configuration;

namespace Automaton.App.Account.Config;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AccountConfig authConfiguration = new();

    public string RegisterUserUrl => authConfiguration.RegisterUserUrl;
    public string UpdateUserProfileUrl => authConfiguration.UpdateUserProfileUrl;
    public string UpdateUserPasswordUrl => authConfiguration.UpdateUserPasswordUrl;
    public string GetUserProfileUrl => authConfiguration.GetUserProfileUrl;
    public string RefreshAccessTokenUrl => authConfiguration.RefreshAccessTokenUrl;
    public int RefreshTokenExpirationMinutesCheck => authConfiguration.RefreshTokenExpirationMinutesCheck;
    public string BaseUrl => authConfiguration.BaseUrl;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AccountConfig)).Bind(authConfiguration);
    }
}
