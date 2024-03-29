﻿using Automaton.Core.Events;
using System.Linq.Dynamic.Core;

namespace Automaton.Core.Models;

public class Workflow
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string StartupDefinitionId { get; set; }

    public IDictionary<string, StepVariable> Variables { get; set; }

    public IDictionary<string, StepVariable> OutputVariables { get; set; }

    public IDictionary<string, StepVariable> InputVariables { get; set; }

    public List<WorkflowDefinition> Definitions { get; set; }

    public event EventHandler<SetVariableNotification> SetWorkflowVariable;

    public Workflow()
    {
        Variables = new Dictionary<string, StepVariable>();
        OutputVariables = new Dictionary<string, StepVariable>();
        InputVariables = new Dictionary<string, StepVariable>();
        Definitions = new List<WorkflowDefinition>();
    }

    public WorkflowDefinition GetStartupDefinition()
    {
        return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
    }

    public bool VariableExists(string key)
    {
        return Variables.ContainsKey(key);
    }

    public StepVariable GetVariable(string key)
    {
        return Variables[key];
    }

    public IEnumerable<StepVariable> GetVariables(IEnumerable<string> names)
    {
        var variables = Variables.Where(x => names.Contains(x.Key)).Select(x => x.Value);

        return variables;
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

        SetWorkflowVariable?.Invoke(this, new SetVariableNotification(variable));
    }
}
