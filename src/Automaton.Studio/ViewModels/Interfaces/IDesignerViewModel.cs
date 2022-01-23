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
        event EventHandler<StepEventArgs> DragStep;
        event EventHandler<StepEventArgs> StepAdded;
        event EventHandler<StepEventArgs> StepRemoved;

        IList<Definition> Definitions { get; }

        void FinalizeStep(Step step);
        void DeleteStep(Step step);
        void CreateStep(StepExplorerModel solutionStep);
        IEnumerable<Step> GetSelectedSteps();
        void UpdateStepConnections(Step step);

        Task LoadFlow(string flowId);
        Task SaveFlow();
    }
}
