using Automaton.Studio.Conductor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IDefinitionService
    {
        Task<IEnumerable<Definition>> List();
        Task<Definition> Get (string id);
        Task<Definition> Create(string name);
        Task Save(Definition flow);
        Task Delete(Guid flowId);
    }
}
