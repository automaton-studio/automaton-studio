using MediatR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Core.Commands
{
    public class UpdateUserInfoCommand: IRequest
    {
        [JsonIgnore]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "First name value is mandatory")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length is 100 characters")]
        public string FirstName { get;  }

        [Required(ErrorMessage = "Last name value is mandatory")]
        [MaxLength(length: 200, ErrorMessage = "Maximum length is 200 characters")]
        public string LastName { get; }

        [Required(ErrorMessage = "Email address value is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; }

        [Required(ErrorMessage = "UserName value is mandatory")]
        public string UserName { get; }

        [JsonConstructor]
        public UpdateUserInfoCommand(Guid? id, string firstName, string lastName, string email, string userName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
        }
    }
}
