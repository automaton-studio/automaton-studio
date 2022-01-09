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

        event EventHandler<StepEventArgs> DragStep;
        event EventHandler<StepEventArgs> StepAdded;
        event EventHandler<StepEventArgs> StepRemoved;

        #endregion

        #region Methods

        void StepDrag(SolutionStep solutionSTep);
        void DeleteStep(Step step);

        Task LoadFlow(string flowId);
        Task SaveFlow();

        #endregion
    }
}
