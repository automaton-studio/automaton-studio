using MediatR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Core.Commands
{
    public class RemoveUserRoleCommand : IRequest
    {
        [JsonIgnore] public Guid UserId { get; }
        [Required] public string RoleName { get; }

        public RemoveUserRoleCommand(Guid userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}