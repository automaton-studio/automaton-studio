using System.Dynamic;

namespace Automaton.Core.Dto;

public class FlowDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string StartupDefinitionId { get; set; }
    public ExpandoObject Variables { get; set; }
    public ExpandoObject OutputVariables { get; set; }
    public List<DefinitionDto> Definitions { get; set; } = new List<DefinitionDto>();
}
