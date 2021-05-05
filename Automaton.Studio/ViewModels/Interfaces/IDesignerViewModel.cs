using Automaton.Studio.Activities;
using Automaton.Studio.Models;
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

        #region Methods

        Task LoadWorkflow(string workflow);

        #endregion
    }
}
