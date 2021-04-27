using System.Threading.Tasks;

namespace Automaton.Runner.Core.Services
{
    public interface IHubService
    {
        Task Connect(JsonWebToken token, string runnerName);
        Task Disconnect();
        Task Ping(string runnerName);
    }
}
