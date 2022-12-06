using Automaton.Core.Enums;
using Automaton.Core.Models;
using Automaton.Studio.Events;
using Microsoft.Scripting.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Automaton.Studio.Domain;

public class StudioDefinition
{
    public string Id { get; set; }

    public int Version { get; set; }

    public string Name { get; set; }

    public List<StudioStep> Steps { get; set; } = new List<StudioStep>();

    public StudioFlow Flow { get; set; }

    public event EventHandler<StepEventArgs> StepAdded;
    public event EventHandler<StepEventArgs> StepRemoved;

    public StudioDefinition()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
        
    public void DeleteStep(StudioStep step)
    {
        Steps.Remove(step);

        StepRemoved?.Invoke(this, new StepEventArgs(step));

        Flow.DeleteVariables(step.GetVariables());

        UpdateStepConnections();
    }

    public void DeleteSteps(int index, int count)
    {
        var steps = Steps.GetRange(index, count);

        Steps.RemoveRange(index, count);

        var variables = new List<StepVariable>();

        foreach (var step in steps)
        {
            StepRemoved?.Invoke(this, new StepEventArgs(step));

            variables.AddRange(step.GetVariables());
        }

        Flow.DeleteVariables(variables);

        UpdateStepConnections();
    }

    public void FinalizeStep(StudioStep step)
    {
        step.MarkAsFinal();
        step.Definition = this;

        UpdateStepConnections();

        StepAdded?.Invoke(this, new StepEventArgs(step));
    }

    public void UpdateStepConnections()
    {
        for(var i = 0; i < Steps.Count; i++)
        {
            Steps[i].NextStepId = i != Steps.Count - 1 ? Steps[i + 1].Id : null;
        }
    }
}
