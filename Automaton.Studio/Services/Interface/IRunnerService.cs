using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IRunnerService
    {
        Runner Get(Guid id);
        IQueryable<Runner> Get();
        int Create(Runner runner);
        bool Exists(Runner runner);
        Task Update(Runner runner);
    }
}
