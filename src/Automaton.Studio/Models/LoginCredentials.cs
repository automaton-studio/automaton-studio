using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Models
{
    public class LoginCredentials
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;
    }
}
