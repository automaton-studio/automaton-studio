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

        StudioFlow StudioFlow { get; set; }
        StudioWorkflow ActiveWorkflow { get; }
        IEnumerable<StudioWorkflow> Workflows { get; }
        IEnumerable<WorkflowRunner> Runners { get; set; }
        IEnumerable<Guid> SelectedRunnerIds { get; set; }

        #endregion

        #region Events

        event EventHandler<ActivityEventArgs> DragActivity;

        #endregion

        #region Methods

        void ActivityDrag(TreeActivity activityModel);
        void FinalizeActivity(StudioActivity activity);       
        void DeleteActivity(StudioActivity activity);

        Task LoadFlow(string flowId);
        Task LoadFlow(Guid flowId);
        Task SaveWorkflow();
        Task RunWorkflow();

        #endregion
    }
}
