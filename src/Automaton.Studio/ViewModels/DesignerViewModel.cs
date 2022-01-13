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
        #region Members

        private readonly IMapper mapper;
        private readonly StepFactory stepFactory;
        private readonly IDefinitionService workflowService;

        #endregion

        #region Properties

        private Definition studioFlow = new();
        public Definition StudioFlow
        {
            get => studioFlow;

            set
            {
                studioFlow = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add
            {
                StudioFlow.StepAdded += value;
            }
            remove
            {

                StudioFlow.StepAdded -= value;
            }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add
            {
                StudioFlow.StepRemoved += value;
            }
            remove
            {

                StudioFlow.StepRemoved -= value;
            }
        }

        #endregion

        #region Constructors

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            IDefinitionService workflowService
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.workflowService = workflowService;

            StudioFlow = new Definition();
        }

        #endregion

        #region Public Methods

        public void StepDrag(SolutionStep solutionStep)
        {
            var step = CreateStep(solutionStep.Name);

            DragStep?.Invoke(this, new StepEventArgs(step));
        }

        private Step CreateStep(string name)
        {
            var step = stepFactory.GetStep(name);

            // Set reference to StudioWorkflow
            step.StudioWorkflow = StudioFlow;

            return step;
        }

        public void AddStep(Step step)
        {
            StudioFlow.Steps.Add(step);
        }

        public void DeleteStep(Step step)
        {
            StudioFlow.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            StudioFlow = await workflowService.Get(flowId);
        }

        public async Task SaveFlow()
        {
            await workflowService.Save(StudioFlow);
        }

        public void FinalizeStep(Step step)
        {
            StudioFlow.FinalizeStep(step);
        }

        public IList<Step> GetSteps()
        {
            return StudioFlow.Steps;
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return StudioFlow.Steps.Where(x => x.IsSelected());
        }

        public string GetDefinitionId()
        {
            return StudioFlow.Id;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
