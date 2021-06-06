using Automaton.Studio.Events;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDesignerViewModel
    {
        #region Properties

        StudioWorkflow Workflow { get; set; }
        IEnumerable<WorkflowRunner> Runners { get; set; }

        #endregion

        #region Events

        event EventHandler<DragActivityEventArgs> DragActivity;

        #endregion

        #region Methods

        Task Initialize(string workflowId);
        void DragTreeActivity(TreeActivity activityModel);

        #endregion
    }
}
