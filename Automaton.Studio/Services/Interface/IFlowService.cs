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
        Flow Get(string name);
        IQueryable<Flow> List();
        int Create(Flow flow);
        bool Exists(Flow flow);
        bool Exists(string name);
        Task Update(Flow flow);
        int Delete(Guid flowId);
    }
}
