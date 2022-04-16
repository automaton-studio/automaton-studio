using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> RefreshToken();
    }
}
