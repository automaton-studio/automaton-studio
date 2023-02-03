namespace Automaton.Client.Auth.Models;

public class AccountConfig
{
    public string RegisterUserUrl { get; set; }
    public string UpdateUserProfileUrl { get; set; }
    public string UpdateUserPasswordUrl { get; set; }
    public string GetUserProfileUrl { get; set; }
    public string LoginUserUrl { get; set; }
    public int RefreshTokenExpirationMinutesCheck { get; set; }
    public string RefreshAccessTokenUrl { get; set; }
}
