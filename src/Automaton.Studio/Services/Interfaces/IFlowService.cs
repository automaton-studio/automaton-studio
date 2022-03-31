using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowService
    {
        Task<IEnumerable<FlowModel>> List();
        Task<Flow> Load(Guid flowId);
        Task<Flow> Create(string name);
        Task Update(Flow flow);
        Task Delete (Guid flowId);
    }
}
