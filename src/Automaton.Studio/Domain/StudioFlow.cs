using Automaton.Core.Models;
using Automaton.Steps;
using System.Collections.Generic;

namespace Automaton.Studio.Domain;

public class StudioFlow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StartupDefinitionId { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public IDictionary<string, object> InputVariables { get; set; }
    public IDictionary<string, object> OutputVariables { get; set; }
    public IList<StudioDefinition> Definitions { get; set; }

    public StudioFlow()
    {
        Name = "Untitled";
        var defaultDefinition = new StudioDefinition { Flow = this };
        StartupDefinitionId = defaultDefinition.Id;
        Definitions = new List<StudioDefinition> { defaultDefinition };
        Variables = new Dictionary<string, object>();
        OutputVariables = new Dictionary<string, object>();
    }


    public StudioDefinition CreateDefinition(string name)
    {
        var definition = new StudioDefinition
        {
            Name = name,
            Flow = this
        };

        Definitions.Add(definition);

        return definition;
    }

    public IEnumerable<string> GetDefinitionNames()
    {
        return Definitions.Select(x => x.Name);
    }

    public StudioDefinition GetStartupDefinition()
    {
        return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
    }

    public void RemoveDefinition(string id)
    {
        var definition = Definitions.SingleOrDefault(x => x.Id.Equals(id));
        Definitions.Remove(definition);
    }

    public void SetVariable(StepVariable variable)
    {
        if (HasStepsUsingVariableOldName(variable))
        {
            Variables.Remove(variable.OldName);
        }

        if (Variables.ContainsKey(variable.Name))
        {
            Variables[variable.Name] = variable;
        }
        else
        {
            Variables.Add(variable.Name, variable.Value);
        }
    }

    public int GetNumberOfSteps<T>()
    {
        var count = Definitions.SelectMany(x => x.Steps).Count(x => x is T);

        return count;
    }

    public void SetInputVariable(string key, object value)
    {
        if (InputVariables.ContainsKey(key))
        {
            InputVariables[key] = value;
        }
        else
        {
            InputVariables.Add(key, value);
        }
    }

    public void SetOutputVariable(string key, object value)
    {
        if (OutputVariables.ContainsKey(key))
        {
            OutputVariables[key] = value;
        }
        else
        {
            OutputVariables.Add(key, value);
        }
    }

    public IEnumerable<string> GetVariableNames()
    {
        return Variables.Keys;
    }

    public IEnumerable<string> GetInputVariableNames()
    {
        return InputVariables.Keys;
    }

    public IEnumerable<string> GetOutputVariableNames()
    {
        return OutputVariables.Keys;
    }


    public void DeleteInputVariable(string variable)
    {
        InputVariables.Remove(variable);
    }

    public void DeleteOutputVariable(string variable)
    {
        OutputVariables.Remove(variable);
    }

    public void DeleteVariable(StepVariable variable)
    {
        if (!HasStepsUsingVariableName(variable))
        {
            if (Variables.ContainsKey(variable.Name))
            {
                Variables.Remove(variable.Name);
            }
        }
    }

    public void DeleteVariables(IEnumerable<StepVariable> variables)
    {
        foreach (var variable in variables)
        {
            DeleteVariable(variable);
        }
    }

    private bool HasStepsUsingVariableOldName(StepVariable variable)
    {
        var stepVariables = Definitions.SelectMany(x => x.Steps).SelectMany(x => x.Outputs);
        var variablesExists = stepVariables.Select(x => x.Value).Any(x => x.Name == variable.OldName);

        return variablesExists;
    }

    private bool HasStepsUsingVariableName(StepVariable variable)
    {
        var stepVariables = Definitions.SelectMany(x => x.Steps).SelectMany(x => x.Outputs);
        var variablesExists = stepVariables.Select(x => x.Value).Any(x => x.Name == variable.Name);

        return variablesExists;
    }
}
