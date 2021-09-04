using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IFlowsViewModel
    {
        #region Properties

        IList<FlowModel> Flows { get; set; }
        IEnumerable<RunnerModel> Runners { get; set; }

        #endregion

        #region Methods

        Task CreateFlow();
        Task RunFlow(FlowModel workflow);
        void DeleteFlow(Guid flowId);

        #endregion
    }
}
