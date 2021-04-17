using Automaton.Runner.Core;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public interface IAuthService
    {
        Task<JsonWebToken> GetToken(UserCredentials userCredentials, string tokenApiUrl);
    }
}
