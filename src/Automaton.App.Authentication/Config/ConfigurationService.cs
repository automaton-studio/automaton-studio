using Microsoft.Extensions.Configuration;

namespace Automaton.App.Authentication.Config;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly LoginConfig loginConfiguration = new();

    public bool UserSignUp => loginConfiguration.UserSignUp;
    public bool NoUserSignUp => !UserSignUp;
    public string LoginUserUrl => loginConfiguration.LoginUserUrl;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(LoginConfig)).Bind(loginConfiguration);
    }
}
