namespace Automaton.Runner.Core.Config
{
    public class AppConfiguration
    {
        public string WebApiUrl { get; set; }
        public string FlowsUrl { get; set; }
        public string LoginUserUrl { get; set; }
        public bool IsDesktop { get; set; }
        public int RefreshTokenExpirationMinutesCheck { get; set; }
        public string RefreshAccessTokenUrl { get; set; }
    }
}
