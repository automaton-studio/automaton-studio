using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.ExecuteFlow;

[StepDescription(
    Name = "ExecuteFlow",
    Type = "ExecuteFlow",
    DisplayName = "Execute Flow",
    Category = "Run flow",
    Description = "Execute flow",
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
        get
        {
            if (Inputs.ContainsKey(nameof(FlowId)))
            {
                var guid = Inputs[nameof(FlowId)].ToString();
                Guid.TryParse(guid, out Guid flowId);
                return flowId;
            }

            return Guid.Empty;
        }
        set
        {
            Inputs[nameof(FlowId)] = value;
        }
    }

    public IList<Variable> InputVariables
    {
        get
        {
            if (Inputs.ContainsKey(nameof(InputVariables)))
            {
                if (Inputs[nameof(InputVariables)] is JArray array)
                {
                    Inputs[nameof(InputVariables)] = array.ToObject<List<Variable>>();
                }
            }
            else
            {
                Inputs[nameof(InputVariables)] = new List<Variable>();
            }

            return Inputs[nameof(InputVariables)] as IList<Variable>;
        }
        set => Inputs[nameof(InputVariables)] = value;
    }

    public IList<Variable> OutputVariables
    {
        get
        {
            if (Inputs.ContainsKey(nameof(OutputVariables)))
            {
                if (Inputs[nameof(OutputVariables)] is JArray array)
                {
                    Inputs[nameof(OutputVariables)] = array.ToObject<List<Variable>>();
                }
            }
            else
            {
                Inputs[nameof(OutputVariables)] = new List<Variable>();
            }

            return Inputs[nameof(OutputVariables)] as IList<Variable>;
        }
        set => Inputs[nameof(OutputVariables)] = value;
    }

    public ExecuteFlowStep(IMapper mapper, FlowsService flowsService)
    {
        this.flowsService = flowsService;
        this.mapper = mapper;
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
}
