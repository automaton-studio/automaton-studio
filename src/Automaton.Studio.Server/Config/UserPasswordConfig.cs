namespace Automaton.Studio.Server.Config;

public class UserPasswordConfig
{
    public bool RequireDigit { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public int RequiredLength { get; set; }
}
