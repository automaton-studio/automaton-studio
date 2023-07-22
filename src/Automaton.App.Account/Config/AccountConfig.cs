namespace Automaton.App.Account.Config;

public class AccountConfig
{
    public string BaseUrl { get; set; }
    public string RegisterUserUrl { get; set; }
    public string UpdateUserProfileUrl { get; set; }
    public string UpdateUserPasswordUrl { get; set; }
    public string GetUserProfileUrl { get; set; }
    public int RefreshTokenExpirationMinutesCheck { get; set; }
    public string RefreshAccessTokenUrl { get; set; }
}
