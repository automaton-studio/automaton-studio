using Automaton.Studio.Attributes;
using Automaton.Studio.Conductor;
using Automaton.Studio.Conductor.Interfaces;
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
        private string message;
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
                InputsDictionary[nameof(Message)] = message;
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
