using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public interface IRegistrationService
    {
        Task Register(string runnerName);
    }
}
