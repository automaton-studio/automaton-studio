namespace Automaton.Studio.Models
{
    public class AuthenticationResult
    {
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Expires { get; set; }
    }
}
