using AutoMapper;
using Automaton.Core.Interfaces;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer
{
    public class DesignerViewModel
    {
        private readonly IMapper mapper;
        private readonly StepFactory stepFactory;
        private readonly FlowService flowService;
        private readonly FlowConvertService flowConvertService;
        private readonly IWorkflowExecutor workflowExecutor;
        private Definition activeDefinition;

        public Flow Flow { get; set; }
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

        public bool CanExecuteFlow
        {
            get
            {
#if DEBUG
                return true;
#else
                return configService.IsDesktop;
#endif
            }
        }

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            FlowService solutionService,
            FlowConvertService flowConvertService,
            IWorkflowExecutor workflowExecutor
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.flowService = solutionService;
            this.flowConvertService = flowConvertService;
            this.workflowExecutor = workflowExecutor;

            Flow = new Flow();
            Definitions = new List<Definition>();
        }

        public void CreateDefinition(string name)
        {
            Definitions.Add(new Definition 
            { 
                Name = name, 
                Flow = this.Flow 
            });
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

        public async Task LoadFlow(Guid flowId)
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
            if (CanExecuteFlow)
            {
                var workflow = flowConvertService.ConvertFlow(Flow);
                await workflowExecutor.Execute(workflow);
            }
        }

        public void FinalizeStep(Step step)
        {
            activeDefinition.FinalizeStep(step);
        }

        public IEnumerable<Step> GetSelectedSteps()
        {
            return activeDefinition.Steps.Where(x => x.IsSelected());
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

        public void UpdateStepConnections()
        {
            activeDefinition.UpdateStepConnections();
        }
    }
}
