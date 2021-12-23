using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDefinitionsViewModel
    {
        #region Methods

        Task<IEnumerable<DefinitionModel>> GetDefinitions();
        Task<DefinitionModel> CreateFlow(string flowName);
        Task RunFlow(DefinitionModel workflow);
        void DeleteFlow(string id);

        #endregion
    }
}
