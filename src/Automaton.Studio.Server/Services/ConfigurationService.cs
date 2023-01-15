using Automaton.Studio.Server.Config;

namespace Automaton.Studio.Server.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly OptionalConfig optionalConfiguration = new();

    public bool NoUserSignUp => !optionalConfiguration.UserSignUp;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(OptionalConfig)).Bind(optionalConfiguration);
    }


}
