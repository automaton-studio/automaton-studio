using Automaton.Studio.Core;
using Automaton.Studio.Core.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Console.WriteLine
{
    [StudioActivity(
        Name = "WriteLine",
        Type = "WriteLine",
        DisplayName = "Write Line",
        Category = "Console",
        Description = "Write text to console",
        Icon = "field-string"
    )]
    public class WriteLineActivity : StudioActivity
    {
        private ActivityDefinitionProperty TextProperty => GetProperty(nameof(Text));
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

        public WriteLineActivity(IActivityTypeDescriber activityDescriber) 
            : base(activityDescriber)
        {
            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Text), string.Empty)
            };
        }

        public override Type GetDesignerComponent()
        {
            return typeof(WriteLineDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(WriteLineProperties);
        }
    }
}
