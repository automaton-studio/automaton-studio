using Automaton.Core.Models;

namespace Automaton.Studio.Domain;

public class StudioDefinition
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<StudioStep> Steps { get; set; } = new List<StudioStep>();

    public StudioFlow Flow { get; set; }

    public StudioDefinition()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
        
    public void DeleteStep(StudioStep step)
    {
        Steps.Remove(step);

        Flow.DeleteVariables(step.GetOutputVariables());

        UpdateStepConnections();
    }

    public void DeleteSteps(IEnumerable<StudioStep> steps)
    {
        foreach (var step in steps)
        {
            Steps.Remove(step);

            Flow.DeleteVariables(step.GetOutputVariables());
        }

        UpdateStepConnections();
    }

    public void DeleteSteps(int index, int count)
    {
        var steps = Steps.GetRange(index, count);

        Steps.RemoveRange(index, count);

        var variablesToDelete = new List<StepVariable>();

        foreach (var step in steps)
        {
            variablesToDelete.AddRange(step.GetOutputVariables());
        }

        Flow.DeleteVariables(variablesToDelete);

        UpdateStepConnections();
    }

    public void CompleteStep(StudioStep step)
    {
        step.Definition = this;

        UpdateStepConnections();
    }

    public void UpdateStepConnections()
    {
        for (var i = 0; i < Steps.Count; i++)
        {
            Steps[i].NextStepId = GetNextStepId(i);
        }
    }

    private string GetNextStepId(int i)
    {
        return i < Steps.Count - 1 ? Steps[i + 1].Id : null;
    }
}
