using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface ISolutionFlowViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IList<WorkflowModel> Workflows { get; set; }
        IEnumerable<string> WorkflowNames { get; }

        #endregion

        #region Methods

        Task LoadFlow(string flowId);
        void RenameWorkflow(string workflowId, string workflowName);
        void AddWorkflow(WorkflowModel workflowModel);

        #endregion
    }
}
