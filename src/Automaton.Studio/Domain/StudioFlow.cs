using Automaton.Core.Models;

namespace Automaton.Studio.Domain;

public class StudioFlow
{
    private StudioStep executingStep;

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string StartupDefinitionId { get; set; }
    public IDictionary<string, StepVariable> Variables { get; set; }
    public IDictionary<string, StepVariable> InputVariables { get; set; }
    public IDictionary<string, StepVariable> OutputVariables { get; set; }
    public List<StudioDefinition> Definitions { get; set; }
    public Dictionary<string, StudioStep> Steps => Definitions
        .SelectMany(d => d.Steps.Select(x => x))
        .ToDictionary(x => x.Id, x => x);

    public StudioFlow()
    {
        Definitions = new List<StudioDefinition>();
        Variables = new Dictionary<string, StepVariable>();
        InputVariables = new Dictionary<string, StepVariable>();
        OutputVariables = new Dictionary<string, StepVariable>();
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

    public StepVariable GetVariable(string name)
    {
        return Variables[name];
    }

    public void SetVariable(StepVariable variable)
    {
        if (Variables.ContainsKey(variable.Name))
        {
            Variables[variable.Name] = variable;
        }
        else
        {
            Variables.Add(variable.Name, variable);
        }
    }

    public void SetInputVariable(StepVariable variable)
    {
        if (InputVariables.ContainsKey(variable.Name))
        {
            InputVariables[variable.Name] = variable;
        }
        else
        {
            InputVariables.Add(variable.Name, variable);
        }
    }

    public void SetOutputVariable(StepVariable variable)
    {
        if (OutputVariables.ContainsKey(variable.Name))
        {
            OutputVariables[variable.Name] = variable;
        }
        else
        {
            OutputVariables.Add(variable.Name, variable);
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

    public void DeleteVariable(string name)
    {
        Variables.Remove(name);
    }

    public void DeleteInputVariable(string variable)
    {
        InputVariables.Remove(variable);
    }

    public void DeleteOutputVariable(string variable)
    {
        OutputVariables.Remove(variable);
    }

    public bool VariableExists(string name)
    {
        return Variables.ContainsKey(name);
    }

    public void DeleteVariable(StepVariable variable)
    {
        if (Variables.ContainsKey(variable.Name))
        {
            Variables.Remove(variable.Name);
        }
    }

    public void DeleteVariables(IEnumerable<StepVariable> variables)
    {
        foreach (var variable in variables)
        {
            DeleteVariable(variable);
        }
    }

    public string GenerateVariableName<T>(string name)
    {
        return $"{name}{GetNumberOfSteps<T>()}";
    }

    public int GetNumberOfSteps<T>()
    {
        var count = Definitions.SelectMany(x => x.Steps).Count(x => x is T);

        return count;
    }

    public void SetExecutingStep(string stepId)
    {
        executingStep?.UnsetExecuting();

        executingStep = Steps[stepId];

        executingStep.SetExecuting();
    }
}
