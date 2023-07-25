using Automaton.Studio.Server.Config;

namespace Automaton.Studio.Server.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AuthenticationConfig authenticationConfig = new();
    private readonly UserPasswordConfig userPasswordConfig = new();

    public bool NoUserSignUp => !authenticationConfig.UserSignUp;
    public bool RequireDigit => userPasswordConfig.RequireDigit;
    public bool RequireLowercase => userPasswordConfig.RequireLowercase;
    public bool RequireUppercase => userPasswordConfig.RequireUppercase;
    public bool RequireNonAlphanumeric => userPasswordConfig.RequireNonAlphanumeric;
    public int RequiredLength => userPasswordConfig.RequiredLength;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.configuration.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);
        this.configuration.GetSection(nameof(UserPasswordConfig)).Bind(userPasswordConfig);
    }
}
