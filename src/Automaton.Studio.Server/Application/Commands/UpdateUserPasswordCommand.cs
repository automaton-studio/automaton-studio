using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Core.Commands
{
    public class UpdateUserPasswordCommand
    {
        [Required(ErrorMessage = "Old password value is mandatory")]
        public string OldPassword { get; }

        [Required(ErrorMessage = "New password value is mandatory")]
        public string NewPassword { get; }

        [JsonConstructor]
        public UpdateUserPasswordCommand(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

