using Automaton.Studio.Activities;
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

        WorkflowModel Workflow { get; set; }
        IList<DynamicActivity> Activities { get; set; }

        #endregion

        #region Events

        event EventHandler<ActivityChangedEventArgs> ActiveItemChanged;

        #endregion

        #region Methods

        Task LoadWorkflow(string workflow);
        void ChangeActiveItem(ActivityTreeModel activityModel);

        #endregion
    }
}
