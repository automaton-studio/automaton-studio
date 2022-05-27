using Automaton.Client.Auth.Models;

namespace Automaton.Client.Auth.Interfaces;

public interface IAuthenticationService
{
    Task Login(LoginDetails loginCredentials);

    Task Logout();

    Task<bool> InitLoggedInAuthorization();   
}
