using System.Threading.Tasks;

namespace Automaton.Runner.Core.Auth
{
    public interface IAuthService
    {
        Task<JsonWebToken> GetToken(UserCredentials userCredentials);
    }
}
