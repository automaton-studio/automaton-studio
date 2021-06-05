using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Console.WriteLine
{
    [StudioActivity(
        Name = "WriteLineActivity",
        DisplayName = "Write Line",
        ElsaName = "WriteLine",
        Category = "Console",
        Description = "Write text to console",
        Icon = "field-string"
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
                OnPropertyChanged();
            }
        }

        public WriteLineActivity(IActivityTypeDescriber activityDescriber) : base(activityDescriber)
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
