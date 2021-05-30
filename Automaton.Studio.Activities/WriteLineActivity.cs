using Automaton.Studio.Activity;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities
{
    [StudioActivity(
        Name = "WriteLineActivity",
        DisplayName = "WriteLine",
        ElsaName = "WriteLine",
        Category = "Console",
        Description = "Write text to console"
    )]
    public class WriteLineActivity : StudioActivity
    {
        private ActivityDefinitionProperty TextProperty => GetDefinitionProperty(nameof(Text));
        public string Text
        {
            get
            {
                return TextProperty.Expressions[TextProperty.Syntax];
            }
            set
            {
                TextProperty.Expressions[TextProperty.Syntax] = value;
            }
        }

        public WriteLineActivity()
        {
            Name = "WriteLineActivity";

            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Text), string.Empty)
            };
        }

        public override Type GetDesignerComponent()
        {
            return typeof(WriteLineDesigner);
        }

        public override Type GetDialogComponent()
        {
            return typeof(WriteLineDialog);
        }
    }
}
