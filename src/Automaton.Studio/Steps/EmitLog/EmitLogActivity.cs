using Automaton.Studio.Attributes;
using Automaton.Studio.Conductor;
using System;

namespace Automaton.Studio.Steps.EmitLog
{
    [StepDescription(
        Name = "EmitLog",
        Type = "EmitLog",
        DisplayName = "Emit Log",
        Category = "Console",
        Description = "Emit log to console",
        Icon = "code"
    )]
    public class EmitLogActivity : Step
    {
        public string Text { get; set; }

        public EmitLogActivity(IActivityDescriptor descriptor) : base(descriptor)
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
