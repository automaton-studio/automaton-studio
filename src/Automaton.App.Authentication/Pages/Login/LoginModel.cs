﻿namespace Automaton.App.Authentication.Pages.Login;

public class LoginModel
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public bool RememberMe { get; set; } = true;
}
