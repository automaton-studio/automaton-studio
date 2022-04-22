using Automaton.Studio.Domain;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowService
    {
        Task<Flow> Load(Guid flowId);
        Task<Flow> Create(string name);
        Task Update(Flow flow);
        Task Delete (Guid flowId);
    }
}
