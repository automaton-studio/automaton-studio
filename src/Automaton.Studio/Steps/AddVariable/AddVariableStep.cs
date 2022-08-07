using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using System;

namespace Automaton.Studio.Steps.AddVariable
{
    [StepDescription(
        Name = "AddVariable",
        Type = "AddVariable",
        DisplayName = "Add Variable",
        Category = "Console",
        Description = "Add Flow variable",
        Icon = "code"
    )]
    public class AddVariableStep : StudioStep
    {
        public string VariableName
        {
            get => Inputs.ContainsKey(nameof(VariableName)) ?
                   Inputs[nameof(VariableName)].ToString() : string.Empty;
            set => Inputs[nameof(VariableName)] = value;
        }

        public string VariableValue
        {
            get => Inputs.ContainsKey(nameof(VariableValue)) ?
                   Inputs[nameof(VariableValue)].ToString() : string.Empty;
            set => Inputs[nameof(VariableValue)] = value;
        }

        public AddVariableStep() 
        {
        }

        public override Type GetDesignerComponent()
        {
            return typeof(AddVariableDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(AddVariableProperties);
        }
    }
}
