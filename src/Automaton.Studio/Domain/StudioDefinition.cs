using Automaton.Core.Models;
using Automaton.Studio.Events;

namespace Automaton.Studio.Domain;

public class StudioDefinition
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<StudioStep> Steps { get; set; } = new List<StudioStep>();

    public StudioFlow Flow { get; set; }

    public event EventHandler<StepEventArgs> StepDeleted;

    public StudioDefinition()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
        
    public void DeleteStep(StudioStep step)
    {
        Steps.Remove(step);

        StepDeleted?.Invoke(this, new StepEventArgs(step));

        Flow.DeleteVariables(step.GetOutputVariables());

        UpdateStepConnections();
    }

    public void DeleteSteps(int index, int count)
    {
        var steps = Steps.GetRange(index, count);

        Steps.RemoveRange(index, count);

        var variables = new List<StepVariable>();

        foreach (var step in steps)
        {
            StepDeleted?.Invoke(this, new StepEventArgs(step));

            variables.AddRange(step.GetOutputVariables());
        }

        Flow.DeleteVariables(variables);

        UpdateStepConnections();
    }

    // TODO! Move Finalize inside Step
    public void FinalizeStep(StudioStep step)
    {
        step.IsNew = false;
        step.Definition = this;

        UpdateStepConnections();
    }

    public void UpdateStepConnections()
    {
        for(var i = 0; i < Steps.Count; i++)
        {
            Steps[i].NextStepId = i != Steps.Count - 1 ? Steps[i + 1].Id : null;
        }
    }
}
