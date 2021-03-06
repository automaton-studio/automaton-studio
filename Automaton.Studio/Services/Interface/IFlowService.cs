using Automaton.Studio.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IFlowService
    {
        Task<StudioFlow> GetAsync (Guid id);
        IEnumerable<StudioFlow> List();
        Task<StudioFlow> Create(string name);
        Task Update(StudioFlow flow);
        Task Delete(Guid flowId);
        bool Exists(string name);
        bool IsUnique(string name);
    }
}
