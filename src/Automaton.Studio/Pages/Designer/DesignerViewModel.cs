using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer;

public class DesignerViewModel
{
    private readonly IMapper mapper;
    private readonly StepFactory stepFactory;
    private readonly FlowService flowService;
    private readonly WorkflowExecuteService workflowExecuteService;

    public StudioFlow Flow { get; set; }
    public IList<StudioDefinition> Definitions { get; set; }
    public StudioDefinition ActiveDefinition { get; set; }

    public event EventHandler<StepEventArgs> StepCreated;
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

        Flow = new StudioFlow();
        Definitions = new List<StudioDefinition>();
    }

    public void CreateDefinition(string name)
    {
        Definitions.Add(new StudioDefinition 
        { 
            Name = name, 
            Flow = this.Flow 
        });
    }

    public void CreateStep(StepExplorerModel solutionStep)
    {
        var step = stepFactory.CreateStep(solutionStep.Name);
        step.Definition = ActiveDefinition;

        StepCreated?.Invoke(this, new StepEventArgs(step));
    }

    public void DeleteStep(StudioStep step)
    {
        ActiveDefinition.Steps.Remove(step); 
    }

    public async Task LoadFlow(Guid flowId)
    {
        Flow = await flowService.Load(flowId);
        Definitions = Flow.Definitions;
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
            var flow = mapper.Map<Flow>(Flow);
            await workflowExecuteService.Execute(flow);
        }
    }

    public void FinalizeStep(StudioStep step)
    {
        step.InvokeFinalize();

        ActiveDefinition.FinalizeStep(step);

        step.InvokeFinalized();
    }

    public IEnumerable<string> GetDefinitionNames()
    {
        return Definitions.Select(x => x.Name);
    }

    public void SetActiveDefinition(StudioDefinition definition)
    {
        ActiveDefinition = definition;
    }

    public void SetActiveDefinition(string id)
    {
        ActiveDefinition = Definitions.SingleOrDefault(x => x.Id == id);
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

    public void UpdateStepConnections()
    {
        ActiveDefinition.UpdateStepConnections();
    }
}
