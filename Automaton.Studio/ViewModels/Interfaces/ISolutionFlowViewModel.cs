using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.ViewModels
{
    public interface ISolutionFlowViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IList<WorkflowModel> Workflows { get; set; }

        #endregion

        #region Methods

        void LoadFlow(string flowId);

        #endregion
    }
}
