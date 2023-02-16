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
    private const string FlowIdName = nameof(FlowId);
    private const string InputVariablesName = nameof(InputVariables);
    private const string OutputVariablesName = nameof(OutputVariables);

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
        set => SetInputVariable(nameof(FlowId), value);
    }

    public IList<StepVariable> InputVariables
    {
        get
        {
            var variables = GetInputVariable(InputVariablesName);
            return variables as IList<StepVariable>;
        }

        set => SetInputVariable(InputVariablesName, value);
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<StepVariable> OutputVariables
    {
        get
        {
            var variables = GetInputVariable(OutputVariablesName);
            return variables as IList<StepVariable>;
        }
        set => SetInputVariable(OutputVariablesName, value);
    }

    public ExecuteFlowStep(IMapper mapper, FlowsService flowsService)
    {
        this.flowsService = flowsService;
        this.mapper = mapper;

        SetInputVariable(OutputVariablesName, new List<StepVariable>());
        SetInputVariable(InputVariablesName, new List<StepVariable>());
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
        var guid = GetStringInputVariable(FlowIdName);
        Guid.TryParse(guid, out Guid flowId);
        return flowId;
    }
}
