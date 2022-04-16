﻿using System;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Domains
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
