using Automaton.Studio.Core;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDesignerViewModel
    {
        #region Properties

        StudioWorkflow StudioWorkflow { get; set; }
        IEnumerable<WorkflowRunner> Runners { get; set; }
        IEnumerable<Guid> SelectedRunnerIds { get; set; }

        #endregion

        #region Events

        event EventHandler<ActivityEventArgs> DragActivity;

        #endregion

        #region Methods

        void DragTreeActivity(TreeActivity activityModel);
        void FinalizeActivity(StudioActivity activity);
        Task LoadWorkflow(string workflowId);
        Task SaveWorkflow();
        Task RunWorkflow();

        #endregion
    }
}
