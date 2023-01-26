using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Core.Commands
{
    public class SignInUserCommand
    {
        [Required(ErrorMessage = "UserName is mandatory")]
        public string UserName { get; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; }

        [JsonConstructor]
        public SignInUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}