using Automaton.Core.Enums;

namespace Automaton.Core.Dto;

public class DefinitionDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

    public TimeSpan? DefaultErrorRetryInterval { get; set; }

    public List<StepDto> Steps { get; set; } = new List<StepDto>();

    public DefinitionDto()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
}
