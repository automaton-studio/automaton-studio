﻿using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;
    }
}