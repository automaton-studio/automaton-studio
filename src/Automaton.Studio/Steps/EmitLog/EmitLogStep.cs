using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using System;

namespace Automaton.Studio.Steps.EmitLog
{
    [StepDescription(
        Name = "EmitLog",
        Type = "EmitLogStep",
        DisplayName = "Emit Log",
        Category = "Console",
        Description = "Emit log to console",
        Icon = "code"
    )]
    public class EmitLogStep : Step
    {
        public string Message
        {
            get
            {
                return InputsDictionary.ContainsKey(nameof(Message)) ?
                    InputsDictionary[nameof(Message)] as string : string.Empty;
            }

            set
            {
                InputsDictionary[nameof(Message)] = value;
            }
        }

        public EmitLogStep(IStepDescriptor descriptor) : base(descriptor)
        {
        }

        public override Type GetDesignerComponent()
        {
            return typeof(EmitLogDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(EmitLogProperties);
        }
    }
}
