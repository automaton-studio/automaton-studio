using Microsoft.AspNetCore.Identity;

namespace Automaton.Studio.Server.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
