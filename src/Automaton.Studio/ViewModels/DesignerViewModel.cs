using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DesignerViewModel : IDesignerViewModel
    {
        private readonly IMapper mapper;
        private readonly StepFactory stepFactory;
        private readonly IFlowService solutionService;
        private Flow flow = new();

        public IList<Definition> Definitions => flow.Definitions;
        public Definition ActiveDefinition => flow.ActiveDefinition;

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add { ActiveDefinition.StepAdded += value; }
            remove { ActiveDefinition.StepAdded -= value; }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add { ActiveDefinition.StepRemoved += value; }
            remove { ActiveDefinition.StepRemoved -= value; }
        }

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            IDefinitionService definitionService,
            IFlowService solutionService
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.solutionService = solutionService;
        }

        public void CreateStep(StepExplorerModel solutionStep)
        {
            var step = stepFactory.GetStep(solutionStep.Name);
            step.Definition = ActiveDefinition;

            DragStep?.Invoke(this, new StepEventArgs(step));
        }

        public void DeleteStep(Step step)
        {
            ActiveDefinition.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            flow = await solutionService.Load(flowId);
        }

        public async Task SaveFlow()
        {
            await solutionService.Save(flow);
        }

        public void FinalizeStep(Step step)
        {
            ActiveDefinition.FinalizeStep(step);
            ActiveDefinition.UpdateStepConnections();
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return ActiveDefinition.Steps.Where(x => x.IsSelected());
        }

        public void UpdateStepConnections(Step step)
        {
            ActiveDefinition.UpdateStepConnections();
        }
    }
}
