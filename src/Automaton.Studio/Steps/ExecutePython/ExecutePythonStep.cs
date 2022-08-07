using Automaton.Core.Models;
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
                   Inputs[nameof(Content)]?.ToString() : string.Empty;
            set => Inputs[nameof(Content)] = value;
        }

        public IList<Variable> InputVariables
        {
            get
            {
                if (Inputs.ContainsKey(nameof(InputVariables)))
                {
                    if (Inputs[nameof(InputVariables)] is JArray array)
                    {
                        Inputs[nameof(InputVariables)] = array.ToObject<List<Variable>>();
                    }
                }
                else
                {
                    Inputs[nameof(InputVariables)] = new List<Variable>();
                }

                return Inputs[nameof(InputVariables)] as IList<Variable>;
            }
            set => Inputs[nameof(InputVariables)] = value;
        }

        public IList<Variable> OutputVariables
        {
            get
            {
                if (Inputs.ContainsKey(nameof(OutputVariables)))
                {
                    if (Inputs[nameof(OutputVariables)] is JArray array)
                    {
                        Inputs[nameof(OutputVariables)] = array.ToObject<List<Variable>>();
                    }
                }
                else
                {
                    Inputs[nameof(OutputVariables)] = new List<Variable>();
                }

                return Inputs[nameof(OutputVariables)] as IList<Variable>;
            }
            set => Inputs[nameof(OutputVariables)] = value;
        }

        public ExecutePythonStep()
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
