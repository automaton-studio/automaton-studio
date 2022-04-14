using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public Guid Id { get; }

        [Required(ErrorMessage = "First name value is mandatory")]
        public string FirstName { get; }

        [Required(ErrorMessage = "Last name value is mandatory")]
        public string LastName { get; }

        [Required(ErrorMessage = "Email address value is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; }

        [Required(ErrorMessage = "UserName value is mandatory")]
        public string UserName { get; }

        [Required(ErrorMessage = "Password value is mandatory")]
        public string Password { get; }

        public DateTime DateOfBirth { get; }

        [JsonConstructor]
        public RegisterUserCommand(Guid id, string firstName, string lastName, DateTime dateOfBirth, string email,
            string password, string userName)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserName = userName;
            DateOfBirth = dateOfBirth;
        }
    }
}