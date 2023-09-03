using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using Automaton.Studio.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner;

public class DesignerViewModel
{
    private const int FlowDelay = 100;

    private readonly IMapper mapper;
    private readonly StepFactory stepFactory;
    private readonly FlowService flowService;
    private readonly StudioFlowExecuteService workflowExecuteService;
    private readonly ConfigurationService configurationService;

    public StudioFlow Flow { get; set; } = new StudioFlow();
    public StudioDefinition ActiveDefinition { get; set; }

    public bool CanExecuteFlow
    {
        get
        {
#if DEBUG
            return true;
#else
            return configurationService.IsDesktop;
#endif
        }
    }

    public event EventHandler<StepEventArgs> StepCreated;

    public DesignerViewModel
    (
        IMapper mapper,
        ConfigurationService configurationService,
        StepFactory stepFactory,
        FlowService flowService,
        StudioFlowExecuteService workflowExecuteService
    )
    {
        this.mapper = mapper;
        this.configurationService = configurationService;
        this.stepFactory = stepFactory;
        this.flowService = flowService;
        this.workflowExecuteService = workflowExecuteService;
    }

    public async Task LoadFlow(Guid flowId)
    {
        Flow = await flowService.Load(flowId);

        ActiveDefinition = Flow.GetStartupDefinition();  
    }

    public async Task SaveFlow()
    {
        await flowService.Update(Flow);
    }

    public async Task RunFlow()
    {
        if (!CanExecuteFlow)
        {
            throw new Exception("Can not execute flow from designer");
        }

        var flow = mapper.Map<Flow>(Flow);
        await workflowExecuteService.Execute(flow, FlowDelay, CancellationToken.None);
    }

    public StudioDefinition CreateDefinition(string name)
    {
        var definition = Flow.CreateDefinition(name);

        return definition;
    }

    public IEnumerable<string> GetDefinitionNames()
    {
        return Flow.GetDefinitionNames();
    }

    public void SetActiveDefinition(StudioDefinition definition)
    {
        ActiveDefinition = definition;
    }

    public void SetActiveDefinition(string id)
    {
        ActiveDefinition = Flow.Definitions.SingleOrDefault(x => x.Id == id);
    }

    public StudioDefinition GetActiveDefinition()
    {
        return ActiveDefinition;
    }

    public string GetStartupDefinitionId()
    {
        return Flow.StartupDefinitionId;
    }

    public void CreateStep(StepExplorerModel customStepModel)
    {
        var step = customStepModel is CustomStepExplorerModel ?
            stepFactory.CreateCustomStep(customStepModel as CustomStepExplorerModel, ActiveDefinition) : 
            stepFactory.CreateStep(customStepModel.Name, ActiveDefinition);
        
        StepCreated?.Invoke(this, new StepEventArgs(step));
    }

    public void DeleteStep(StudioStep step)
    {
        ActiveDefinition.DeleteStep(step);
    }

    public void FinalizeStep(StudioStep step)
    {
        ActiveDefinition.CompleteStep(step);
    }

    public void UpdateStepConnections()
    {
        ActiveDefinition.UpdateStepConnections();
    }

    public void SetExecutingStep(string stepId)
    {
        Flow.SetExecutingStep(stepId);
    }
}
