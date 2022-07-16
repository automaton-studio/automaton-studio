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

        public IList<StepVariable> InputVariables
        {
            get
            {
                if (Inputs.ContainsKey(nameof(InputVariables)))
                {
                    if (Inputs[nameof(InputVariables)] is JArray array)
                    {
                        Inputs[nameof(InputVariables)] = array.ToObject<List<StepVariable>>();
                    }
                }
                else
                {
                    Inputs[nameof(InputVariables)] = new List<StepVariable>();
                }

                return Inputs[nameof(InputVariables)] as IList<StepVariable>;
            }
            set => Inputs[nameof(InputVariables)] = value;
        }

        public IList<StepVariable> OutputVariables
        {
            get
            {
                if (Outputs.ContainsKey(nameof(OutputVariables)))
                {
                    if (Outputs[nameof(OutputVariables)] is JArray array)
                    {
                        Outputs[nameof(OutputVariables)] = array.ToObject<List<StepVariable>>();
                    }
                }
                else
                {
                    Outputs[nameof(OutputVariables)] = new List<StepVariable>();
                }

                return Outputs[nameof(OutputVariables)] as IList<StepVariable>;
            }
            set => Outputs[nameof(OutputVariables)] = value;
        }

        public ExecutePythonStep(IStepDescriptor descriptor) 
            : base(descriptor)
        {
            Inputs[nameof(InputVariables)] = new List<StepVariable>();
            Outputs[nameof(OutputVariables)] = new List<StepVariable>();
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

    public class StepVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
    };
}
