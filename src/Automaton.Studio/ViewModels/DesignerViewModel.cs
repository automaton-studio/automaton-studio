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
        private StudioFlow studioFlow = new();

        #endregion

        #region Events

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add
            {
                studioFlow.ActiveWorkflow.StepAdded += value;
            }
            remove
            {

                studioFlow.ActiveWorkflow.StepAdded -= value;
            }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add
            {
                studioFlow.ActiveWorkflow.StepRemoved += value;
            }
            remove
            {

                studioFlow.ActiveWorkflow.StepRemoved -= value;
            }
        }

        /// <summary>
        /// Flow workflows
        /// </summary>
        public IList<Definition> Workflows => studioFlow.Workflows;

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
            step.StudioWorkflow = studioFlow.ActiveWorkflow;

            return step;
        }

        public void AddStep(Step step)
        {
            studioFlow.ActiveWorkflow.Steps.Add(step);
        }

        public void DeleteStep(Step step)
        {
            studioFlow.ActiveWorkflow.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            //studioFlow = await workflowService.Get(flowId);
        }

        public async Task SaveFlow()
        {
            await workflowService.Save(studioFlow.ActiveWorkflow);
        }

        public void FinalizeStep(Step step)
        {
            studioFlow.ActiveWorkflow.FinalizeStep(step);
        }

        public IList<Step> GetSteps()
        {
            return studioFlow.ActiveWorkflow.Steps;
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return studioFlow.ActiveWorkflow.Steps.Where(x => x.IsSelected());
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
