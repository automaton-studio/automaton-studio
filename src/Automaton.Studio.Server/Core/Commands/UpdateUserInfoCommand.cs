﻿using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class UpdateUserInfoCommand: IRequest
    {
        [JsonIgnore]
        public Guid Id { get; }
        [Required(ErrorMessage = "First name value is mandatory")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length is 100 characters")]
        public String FirstName { get;  }
        [Required(ErrorMessage = "Last name value is mandatory")]
        [MaxLength(length: 200, ErrorMessage = "Maximum length is 200 characters")]
        public String LastName { get; }

        public DateTime DateOfBirth { get; }
        [MaxLength(length: 500, ErrorMessage = "Maximum length is 500 characters")]
        public String Occupation { get; }
        [MaxLength(length: 500, ErrorMessage = "Maximum length is 500 characters")]
        public String Education { get; }

        [JsonConstructor]
        public UpdateUserInfoCommand(Guid id, String firstName, String lastName, DateTime dateOfBirth, String occupation, String education)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Occupation = occupation;
            this.Education = education;
        }

    }
}
