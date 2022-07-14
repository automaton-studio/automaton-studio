using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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

        public IList<InputVariable> InputVariables
        {
            get
            {
                if (Inputs.ContainsKey(nameof(InputVariables)))
                {
                    if (Inputs[nameof(InputVariables)] is IList<InputVariable>)
                    {
                        return Inputs[nameof(InputVariables)] as IList<InputVariable>;
                    }
                    else if (Inputs[nameof(InputVariables)] is JArray array)
                    {
                        Inputs[nameof(InputVariables)] = array.ToObject<List<InputVariable>>();
                    }
                    else
                    {
                        throw new Exception("Unknown InputVariables serialization");
                    }
                }
                
                return new List<InputVariable>();
            }
            set => Inputs[nameof(InputVariables)] = value;
        }

        public IList<string> OutputVariables => Variables;

        public ExecutePythonStep(IStepDescriptor descriptor) 
            : base(descriptor)
        {
            Inputs[nameof(InputVariables)] = new List<InputVariable>();
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

    public class InputVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
    };
}
