using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Models;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDesignerViewModel
    {
        #region Properties

        Definition StudioFlow { get; set; }

        #endregion

        #region Events

        event EventHandler<StepEventArgs> DragActivity;
        event EventHandler<StepEventArgs> ActivityAdded;
        event EventHandler<StepEventArgs> ActivityRemoved;

        #endregion

        #region Methods

        void ActivityDrag(SolutionStep activityModel);
        void DeleteActivity(Step activity);

        Task LoadFlow(string flowId);
        Task SaveFlow();

        #endregion
    }
}
