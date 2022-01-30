using AutoMapper;
using Automaton.Studio.Components.Explorer.FlowExplorer;
using Automaton.Studio.Domain;
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

        public Flow Flow { get; set; }
        public Definition ActiveDefinition { get; set;  }
        public IList<Definition> Definitions { get; set; }

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

        public void NewDefinition(string name)
        {
            Definitions.Add(new Definition { Name = name });
        }

        public void CreateStep(StepExplorerModel solutionStep)
        {
            var step = stepFactory.CreateStep(solutionStep.Name);
            step.Definition = ActiveDefinition;

            DragStep?.Invoke(this, new StepEventArgs(step));
        }

        public void DeleteStep(Step step)
        {
            ActiveDefinition.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            Flow = await solutionService.Load(flowId);

            Definitions = Flow.Definitions;
            ActiveDefinition = Flow.ActiveDefinition;
        }

        public bool FlowInitialized()
        {
            return Flow != null;
        }

        public async Task SaveFlow()
        {
            await solutionService.Save(Flow);
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

        public IEnumerable<string> GetDefinitionNames()
        {
            return Definitions.Select(x => x.Name);
        }
    }
}
