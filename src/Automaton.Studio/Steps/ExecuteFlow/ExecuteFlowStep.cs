using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.ExecuteFlow;

[StepDescription(
    Name = "ExecuteFlow",
    Type = "ExecuteFlow",
    DisplayName = "Execute Flow",
    Category = "Run flow",
    Description = "Executes a flow",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "apartment"
)]
public class ExecuteFlowStep : StudioStep
{
    private readonly IMapper mapper;
    private readonly FlowsService flowsService;

    public IEnumerable<ExecuteFlowModel> Flows { get; set; } = new List<ExecuteFlowModel>();

    public string FlowName
    {
        get
        {
            var flow = Flows.SingleOrDefault(x => x.Id == FlowId);
            return flow != null ? flow.Name : string.Empty;
        }
    }

    public Guid FlowId
    {
        get => GetFlowIdFromInput();
        set => SetInputValue(nameof(FlowId), value);
    }

    public IList<StepVariable> InputVariables
    {
        get => GetInputValue(nameof(InputVariables)) as IList<StepVariable>;
        set => SetInputValue(nameof(InputVariables), value);
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<StepVariable> OutputVariables
    {
        get => GetInputValue(nameof(OutputVariables)) as IList<StepVariable>;
        set => SetInputValue(nameof(OutputVariables), value);
    }

    public ExecuteFlowStep(IMapper mapper, FlowsService flowsService)
    {
        this.flowsService = flowsService;
        this.mapper = mapper;

        SetInputValue(nameof(FlowId), Guid.Empty);
        SetInputValue(nameof(OutputVariables), new List<StepVariable>());
        SetInputValue(nameof(InputVariables), new List<StepVariable>());
    }

    public void OnFocus()
    {
        var flowsInfo = Task.Run(async () => await this.flowsService.List()).Result;
        Flows = this.mapper.Map<ICollection<ExecuteFlowModel>>(flowsInfo);
    }

    public override Type GetDesignerComponent()
    {
        return typeof(ExecuteFlowDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(ExecuteFlowProperties);
    }

    private Guid GetFlowIdFromInput()
    {
        var guid = GetInputValue(nameof(FlowId)) as string;
        Guid.TryParse(guid, out Guid flowId);
        return flowId;
    }
}
