using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class RemoveUserRoleCommand : IRequest
    {
        [JsonIgnore] public Guid UserId { get; }
        [Required] public String RoleName { get; }

        public RemoveUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}