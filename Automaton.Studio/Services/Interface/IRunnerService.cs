using System;
using System.Linq;

namespace Automaton.Studio.Services
{
    public interface IRunnerService
    {
        Runner Get(Guid id);
        IQueryable<Runner> Get();
        int Add(Runner runner);
    }
}
