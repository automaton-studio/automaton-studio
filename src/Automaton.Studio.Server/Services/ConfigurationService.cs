using Automaton.Studio.Server.Config;

namespace Automaton.Studio.Server.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly OptionalConfig optionalConfig = new();
    private readonly UserPasswordConfig userPasswordConfig = new();

    public bool NoUserSignUp => !optionalConfig.UserSignUp;
    public bool RequireDigit => userPasswordConfig.RequireDigit;
    public bool RequireLowercase => userPasswordConfig.RequireLowercase;
    public bool RequireUppercase => userPasswordConfig.RequireUppercase;
    public bool RequireNonAlphanumeric => userPasswordConfig.RequireNonAlphanumeric;
    public int RequiredLength => userPasswordConfig.RequiredLength;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(OptionalConfig)).Bind(optionalConfig);
        this.configuration.GetSection(nameof(UserPasswordConfig)).Bind(userPasswordConfig);
    }
}
