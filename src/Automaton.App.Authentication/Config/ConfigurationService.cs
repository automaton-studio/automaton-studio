using Microsoft.Extensions.Configuration;

namespace Automaton.App.Authentication.Config;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AuthenticationConfig authenticationConfig = new();

    public bool UserSignUp => authenticationConfig.UserSignUp;
    public bool NoUserSignUp => !UserSignUp;
    public string LoginUserUrl => authenticationConfig.LoginUserUrl;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);
    }
}
