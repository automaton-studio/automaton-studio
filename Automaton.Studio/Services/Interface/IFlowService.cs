using Automaton.Studio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IFlowService
    {
        Flow Get(Guid id);
        IQueryable<Flow> List();
        int Create(Flow flow);
        bool Exists(Flow flow);
        Task Update(Flow flow);
        Task RunFlow(Guid flowId, IEnumerable<Guid> runnerIds);
    }
}
