namespace Automaton.Client.Auth.Models
{
    public class AuthenticationConfiguration
    {
        public string WebApiUrl { get; set; }
        public string LoginUserUrl { get; set; }
        public int RefreshTokenExpirationMinutesCheck { get; set; }
        public string RefreshAccessTokenUrl { get; set; }
    }
}
