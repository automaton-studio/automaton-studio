using Automaton.Studio.Events;
using Automaton.Studio.Models;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDesignerViewModel
    {
        #region Properties

        StudioWorkflow Workflow { get; set; }

        #endregion

        #region Events

        event EventHandler<DragActivityEventArgs> DragActivity;

        #endregion

        #region Methods

        Task LoadWorkflow(string workflow);
        void DragTreeActivity(TreeActivity activityModel);

        #endregion
    }
}
