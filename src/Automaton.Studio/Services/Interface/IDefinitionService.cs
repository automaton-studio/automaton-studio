using Automaton.Studio.Conductor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IDefinitionService
    {
        Task<Definition> Get (Guid id);
        Task<IEnumerable<Definition>> List();
        Task<Definition> Create(string name);
        Task Update(Definition flow);
        Task Delete(Guid flowId);
    }
}
