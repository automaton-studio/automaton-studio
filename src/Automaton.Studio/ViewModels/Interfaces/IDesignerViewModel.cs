using Automaton.Studio.Domain;
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

        Flow Flow { get; set; }
        IList<Definition> Definitions { get; set; }
        Definition ActiveDefinition { get; set; }

        void FinalizeStep(Step step);
        void DeleteStep(Step step);
        void CreateStep(StepExplorerModel solutionStep);
        IEnumerable<Step> GetSelectedSteps();
        void UpdateStepConnections(Step step);

        void NewDefinition(string name);
        IEnumerable<string> GetDefinitionNames();

        bool FlowInitialized();
        Task LoadFlow(string flowId);
        Task SaveFlow();
    }
}
