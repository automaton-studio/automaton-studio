using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IDesignerViewModel
    {
        #region Events

        event EventHandler<StepEventArgs> DragStep;
        event EventHandler<StepEventArgs> StepAdded;
        event EventHandler<StepEventArgs> StepRemoved;

        #endregion

        #region Methods

        string GetDefinitionId();
        IList<Step> GetSteps();
        IEnumerable<Step> GetSelectedSteps();

        void StepDrag(SolutionStep solutionSTep);
        void FinalizeStep(Step step);
        void DeleteStep(Step step);

        Task LoadFlow(string flowId);
        Task SaveFlow();

        #endregion
    }
}
