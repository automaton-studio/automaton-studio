using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DesignerViewModel : IDesignerViewModel, INotifyPropertyChanged
    {
        private readonly IMapper mapper;
        private readonly StepFactory stepFactory;
        private readonly IDefinitionService definitionService;
        private readonly Flow flow = new();

        public IList<Definition> Definitions => flow.Definitions;

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add { flow.ActiveDefinition.StepAdded += value; }
            remove { flow.ActiveDefinition.StepAdded -= value; }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add { flow.ActiveDefinition.StepRemoved += value; }
            remove { flow.ActiveDefinition.StepRemoved -= value; }
        }

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            IDefinitionService workflowService
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.definitionService = workflowService;
        }

        public void StepDrag(StepExplorerModel solutionStep)
        {
            var step = CreateStep(solutionStep.Name);

            DragStep?.Invoke(this, new StepEventArgs(step));
        }

        private Step CreateStep(string name)
        {
            var step = stepFactory.GetStep(name);

            // Set reference to StudioWorkflow
            step.StudioWorkflow = flow.ActiveDefinition;

            return step;
        }

        public void AddStep(Step step)
        {
            flow.ActiveDefinition.Steps.Add(step);
        }

        public void DeleteStep(Step step)
        {
            flow.ActiveDefinition.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            //studioFlow = await workflowService.Get(flowId);
        }

        public async Task SaveFlow()
        {
            await definitionService.Save(flow.ActiveDefinition);
        }

        public void FinalizeStep(Step step)
        {
            flow.ActiveDefinition.FinalizeStep(step);
        }

        public IList<Step> GetSteps()
        {
            return flow.ActiveDefinition.Steps;
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return flow.ActiveDefinition.Steps.Where(x => x.IsSelected());
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
