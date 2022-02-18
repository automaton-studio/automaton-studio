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
    public class AddVariableStep : Step
    {
        public string VariableName
        {
            get => InputsDictionary.ContainsKey(nameof(VariableName)) ?
                   InputsDictionary[nameof(VariableName)] as string : string.Empty;
            set => InputsDictionary[nameof(VariableName)] = value;
        }

        public string VariableValue
        {
            get => InputsDictionary.ContainsKey(nameof(VariableValue)) ?
                   InputsDictionary[nameof(VariableValue)] as string : string.Empty;
            set => InputsDictionary[nameof(VariableValue)] = value;
        }

        public AddVariableStep(IStepDescriptor descriptor) 
            : base(descriptor)
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
