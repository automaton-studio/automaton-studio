namespace Automaton.Client.Auth.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
        public string UserId { get; set; }

        public bool IsValid()
        {
            var valid = !string.IsNullOrEmpty(AccessToken) && 
                !string.IsNullOrEmpty(RefreshToken) && 
                !string.IsNullOrEmpty(UserId) &&
                new DateTime(Expires) <= DateTime.UtcNow;

            return valid;
        }
    }
}
