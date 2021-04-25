using Automaton.Runner.Core;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public interface IAuthService
    {
        JsonWebToken Token { get; set; }

        Task SignIn(string username, string password);
    }
}
