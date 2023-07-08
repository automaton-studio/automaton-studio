using AutoMapper;
using Automaton.Core.Logs;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner;

public class DesignerViewModel
{
    private readonly IMapper mapper;
    private readonly StepFactory stepFactory;
    private readonly FlowService flowService;
    private readonly WorkflowExecuteService workflowExecuteService;
    private WorkflowSink workflowSink;

    public StudioFlow Flow { get; set; } = new StudioFlow();
    public StudioDefinition ActiveDefinition { get; set; }
    public IList<Serilog.Events.LogEvent> Logs => workflowSink.Logs;

    public event EventHandler<StepEventArgs> StepCreated;

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
        WorkflowExecuteService workflowExecuteService,
        WorkflowSink workflowSink
    )
    {
        this.mapper = mapper;
        this.stepFactory = stepFactory;
        this.flowService = flowService;
        this.workflowExecuteService = workflowExecuteService;
        this.workflowSink = workflowSink;
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
        
        step.IsNew = true;

        step.InvokeCreated();

        StepCreated?.Invoke(this, new StepEventArgs(step));
    }

    public void DeleteStep(StudioStep step)
    {
        ActiveDefinition.DeleteStep(step);
    }

    public void FinalizeStep(StudioStep step)
    {
        ActiveDefinition.FinalizeStep(step);

        step.InvokeFinalized();
    }

    public void UpdateStepConnections()
    {
        ActiveDefinition.UpdateStepConnections();
    }
}
