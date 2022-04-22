using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowsService
    {
        Task<ICollection<FlowDetails>> List();
    }
}
