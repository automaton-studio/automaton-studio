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
        IEnumerable<WorkflowRunner> Runners { get; set; }

        #endregion

        #region Methods

        void Initialize();
        Task CreateNewFlow();
        Task RunWorkflow(FlowModel workflow);
        void DeleteFlow(Guid flowId);

        #endregion
    }
}
