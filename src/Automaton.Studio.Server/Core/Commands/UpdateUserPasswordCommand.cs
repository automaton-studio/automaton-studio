using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class UpdateUserPasswordCommand : IRequest
    {
        public Guid UserId { get; }

        [Required(ErrorMessage = "Old password value is mandatory")]
        public String OldPassword { get; }

        [Required(ErrorMessage = "New password value is mandatory")]
        public String NewPassword { get; }

        [JsonConstructor]
        public UpdateUserPasswordCommand(Guid userId, String oldPassword, String newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

