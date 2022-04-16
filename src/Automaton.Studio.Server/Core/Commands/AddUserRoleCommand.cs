using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class AddUserRoleCommand : IRequest
    {
        [JsonIgnore] 
        public Guid UserId { get; }

        [Required] public String RoleName { get; }

        [JsonConstructor]
        public AddUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}