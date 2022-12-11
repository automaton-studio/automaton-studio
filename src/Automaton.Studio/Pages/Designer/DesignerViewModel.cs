using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer;

public class DesignerViewModel
{
    private readonly IMapper mapper;
    private readonly StepFactory stepFactory;
    private readonly FlowService flowService;
    private readonly WorkflowExecuteService workflowExecuteService;

    public StudioFlow Flow { get; set; } = new StudioFlow();
    public StudioDefinition ActiveDefinition { get; set; }

    public event EventHandler<StepEventArgs> StepCreated;
    public event EventHandler<StepEventArgs> StepDeleted;

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
        FlowService flowService,
        WorkflowExecuteService workflowExecuteService
    )
    {
        this.mapper = mapper;
        this.stepFactory = stepFactory;
        this.flowService = flowService;
        this.workflowExecuteService = workflowExecuteService;
    }

    public async Task LoadFlow(Guid flowId)
    {
        Flow = await flowService.Load(flowId);

        foreach (var definition in Flow.Definitions)
        {
            definition.StepDeleted += OnStepDeleted;
        }

        ActiveDefinition = Flow.GetStartupDefinition();  
    }

    public async Task SaveFlow()
    {
        await flowService.Update(Flow);
    }

    public async Task RunFlow()
    {
        if (CanExecuteFlow)
        {
            // TODO! Need a way to update Flow variables on the fly
            // during Workflow execution. This way we can introduce a
            // Debug functionality where user can add breakpoints and
            // investigate the values of Flow variables
            var flow = mapper.Map<Flow>(Flow);
            await workflowExecuteService.Execute(flow);
        }
    }

    public void CreateDefinition(string name)
    {
        var definition = Flow.CreateDefinition(name);
        definition.StepDeleted += OnStepDeleted;
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

    public string GetActiveDefinitionId()
    {
        return ActiveDefinition != null ? ActiveDefinition.Id : string.Empty;
    }

    public string GetStartupDefinitionId()
    {
        return Flow.StartupDefinitionId;
    }

    public void CreateStep(string name)
    {
        var step = stepFactory.CreateStep(name);
        step.Definition = ActiveDefinition;
        step.InvokeCreated();

        StepCreated?.Invoke(this, new StepEventArgs(step));
    }

    public void DeleteStep(StudioStep step)
    {
        ActiveDefinition.DeleteStep(step);
    }

    public void FinalizeStep(StudioStep step)
    {
        step.InvokeFinalize();

        ActiveDefinition.FinalizeStep(step);

        step.InvokeFinalized();
    }

    public void UpdateStepConnections()
    {
        ActiveDefinition.UpdateStepConnections();
    }

    private void OnStepDeleted(object sender, StepEventArgs e)
    {
        StepDeleted?.Invoke(this, new StepEventArgs(e.Step));
    }
}
