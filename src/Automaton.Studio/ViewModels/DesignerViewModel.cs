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
        private Definition studioFlow = new();

        #endregion

        #region Events

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add
            {
                studioFlow.StepAdded += value;
            }
            remove
            {

                studioFlow.StepAdded -= value;
            }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add
            {
                studioFlow.StepRemoved += value;
            }
            remove
            {

                studioFlow.StepRemoved -= value;
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

            studioFlow = new Definition();
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
            step.StudioWorkflow = studioFlow;

            return step;
        }

        public void AddStep(Step step)
        {
            studioFlow.Steps.Add(step);
        }

        public void DeleteStep(Step step)
        {
            studioFlow.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            studioFlow = await workflowService.Get(flowId);
        }

        public async Task SaveFlow()
        {
            await workflowService.Save(studioFlow);
        }

        public void FinalizeStep(Step step)
        {
            studioFlow.FinalizeStep(step);
        }

        public IList<Step> GetSteps()
        {
            return studioFlow.Steps;
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return studioFlow.Steps.Where(x => x.IsSelected());
        }

        public string GetDefinitionId()
        {
            return studioFlow.Id;
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
