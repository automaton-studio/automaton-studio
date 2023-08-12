using Automaton.Studio.Server.Config;

namespace Automaton.Studio.Server.Services;

public class ConfigurationService
{
    private readonly IConfiguration configuration;
    private readonly AppConfig appConfig = new();
    private readonly UserPasswordConfig userPasswordConfig = new();

    public const string MsSqlDatabaseType = "MsSql";
    public const string MySqlDatabaseType = "MySql";

    public string DatabaseType => appConfig.DatabaseType;
    public int RefreshTokenLifetime => appConfig.RefreshTokenLifetime;
    public bool NoUserSignUp => !appConfig.UserSignUp;

    public bool RequireDigit => userPasswordConfig.RequireDigit;
    public bool RequireLowercase => userPasswordConfig.RequireLowercase;
    public bool RequireUppercase => userPasswordConfig.RequireUppercase;
    public bool RequireNonAlphanumeric => userPasswordConfig.RequireNonAlphanumeric;
    public int RequiredLength => userPasswordConfig.RequiredLength;

    public ConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.GetSection(nameof(AppConfig)).Bind(appConfig);
        this.configuration.GetSection(nameof(UserPasswordConfig)).Bind(userPasswordConfig);
    }

    public bool IsDatabaseTypeMsSql()
    {
        return DatabaseType == MsSqlDatabaseType;
    }

    public bool IsDatabaseTypeMySql()
    {
        return DatabaseType == MySqlDatabaseType;
    }
}
