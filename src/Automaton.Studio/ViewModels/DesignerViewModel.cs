using AutoMapper;
using Automaton.Core.Interfaces;
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
        private readonly IFlowService flowService;
        private readonly IWorkflowExecutor workflowExecutor;

        public Flow Flow { get; set; }
        public Definition activeDefinition { get; set;  }
        public IList<Definition> Definitions { get; set; }

        public event EventHandler<StepEventArgs> DragStep;
        public event EventHandler<StepEventArgs> StepAdded
        {
            add { activeDefinition.StepAdded += value; }
            remove { activeDefinition.StepAdded -= value; }
        }

        public event EventHandler<StepEventArgs> StepRemoved
        {
            add { activeDefinition.StepRemoved += value; }
            remove { activeDefinition.StepRemoved -= value; }
        }

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            IFlowService solutionService,
            IWorkflowExecutor workflowExecutor
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.flowService = solutionService;
            this.workflowExecutor = workflowExecutor;

            Flow = new Flow();
            Definitions = new List<Definition>();
        }

        public void NewDefinition(string name)
        {
            Definitions.Add(new Definition { Name = name });
        }

        public void CreateStep(StepExplorerModel solutionStep)
        {
            var step = stepFactory.CreateStep(solutionStep.Name);
            step.Definition = activeDefinition;

            DragStep?.Invoke(this, new StepEventArgs(step));
        }

        public void DeleteStep(Step step)
        {
            activeDefinition.Steps.Remove(step); 
        }

        public async Task LoadFlow(string flowId)
        {
            Flow = await flowService.Load(flowId);

            Definitions = Flow.Definitions;
            activeDefinition = Flow.GetStartupDefinition();
        }

        public async Task SaveFlow()
        {
            await flowService.Update(Flow);
        }

        public async Task RunFlow()
        {
            var workflow = flowService.ConvertFlow(Flow);
            await workflowExecutor.Execute(workflow);
        }

        public void FinalizeStep(Step step)
        {
            activeDefinition.FinalizeStep(step);
            activeDefinition.UpdateStepConnections();
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return activeDefinition.Steps.Where(x => x.IsSelected());
        }

        public void UpdateStepConnections(Step step)
        {
            activeDefinition.UpdateStepConnections();
        }

        public IEnumerable<string> GetDefinitionNames()
        {
            return Definitions.Select(x => x.Name);
        }

        public void SetActiveDefinition(Definition definition)
        {
            activeDefinition = definition;
        }

        public void SetActiveDefinition(string id)
        {
            activeDefinition = Definitions.SingleOrDefault(x => x.Id == id);
        }

        public Definition GetActiveDefinition()
        {
            return activeDefinition;
        }

        public string GetActiveDefinitionId()
        {
            return activeDefinition != null ? activeDefinition.Id : string.Empty;
        }

        public string GetStartupDefinitionId()
        {
            return Flow.StartupDefinitionId;
        }
    }
}
