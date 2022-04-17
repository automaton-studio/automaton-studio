using Automaton.Studio.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> Login(LoginModel userCredentials);
        Task Logout();
    }
}
