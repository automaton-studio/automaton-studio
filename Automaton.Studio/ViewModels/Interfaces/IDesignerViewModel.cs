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
        IList<StudioWorkflow> Workflows { get; }
        IEnumerable<RunnerModel> Runners { get; set; }
        IEnumerable<Guid> SelectedRunnerIds { get; set; }

        #endregion

        #region Events

        event EventHandler<ActivityEventArgs> DragActivity;
        event EventHandler<ActivityEventArgs> ActivityAdded;
        event EventHandler<ActivityEventArgs> ActivityRemoved;

        #endregion

        #region Methods

        void ActivityDrag(ActivityModel activityModel);
        void FinalizeActivity(StudioActivity activity);       
        void DeleteActivity(StudioActivity activity);

        Task LoadFlow(string flowId);
        Task LoadFlow(Guid flowId);
        Task SaveFlow();
        Task RunWorkflow();
        void AddWorkflow(StudioWorkflow newStudioWorkflow);

        #endregion
    }
}
