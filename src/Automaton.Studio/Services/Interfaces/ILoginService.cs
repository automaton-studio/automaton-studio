using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface ILoginService
    {
        Task Login(string userName, string password);
    }
}
