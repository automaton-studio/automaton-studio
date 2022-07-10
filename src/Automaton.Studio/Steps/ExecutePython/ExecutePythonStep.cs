using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using System;

namespace Automaton.Studio.Steps.ExecutePython
{
    [StepDescription(
        Name = "ExecutePython",
        Type = "ExecutePython",
        DisplayName = "Execute Python",
        Category = "Console",
        Description = "Execute Python script",
        Icon = "code"
    )]
    public class ExecutePythonStep : StudioStep
    {
        public string Content
        {
            get => Inputs.ContainsKey(nameof(Content)) ?
                   Inputs[nameof(Content)].ToString() : string.Empty;
            set => Inputs[nameof(Content)] = value;
        }

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

        public ExecutePythonStep(IStepDescriptor descriptor) 
            : base(descriptor)
        {
        }

        public override Type GetDesignerComponent()
        {
            return typeof(ExecutePythonDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(ExecutePythonProperties);
        }
    }
}
