using MediatR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Core.Commands
{
    public class UpdateUserPasswordCommand : IRequest
    {
        public Guid UserId { get; }

        [Required(ErrorMessage = "Old password value is mandatory")]
        public string OldPassword { get; }

        [Required(ErrorMessage = "New password value is mandatory")]
        public string NewPassword { get; }

        [JsonConstructor]
        public UpdateUserPasswordCommand(Guid userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

