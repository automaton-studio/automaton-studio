﻿using System;
using Common.Authentication;
using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Commands
{
    public class SignInUserCommand : IRequest<JsonWebToken>
    {
        public string Password { get; }
        public string UserName { get; }

        [JsonConstructor]
        public SignInUserCommand(string password, string userName)
        {
            Password = password;
            UserName = userName;
        }
    }
}