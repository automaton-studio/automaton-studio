using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IRunnerService
    {
        Runner Get(Guid id);
        IEnumerable<Runner> List();
        int Create(string name);
        bool Exists(string name);
        Task Update(Runner runner);

        Task RunWorkflow(string workflowId, IEnumerable<Guid> runnerIds);
    }
}
