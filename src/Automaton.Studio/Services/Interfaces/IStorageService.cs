using Automaton.Studio.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> GetAuthToken();
        Task<string> GetRefreshToken();
        Task SetAuthAndRefreshTokens(JsonWebToken token);
        Task DeleteAuthAndRefreshTokens();
    }
}
