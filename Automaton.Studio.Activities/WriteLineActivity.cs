using Automaton.Studio.Activities.Attributes;
using Automaton.Studio.Activity;
using Automaton.Studio.Models;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities
{
    [Activity(
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

        public override IStudioActivityModel Model { get; set; } = new WriteLineModel();

        public WriteLineActivity()
        {
            Name = "WriteLineActivity";

            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Text), string.Empty)
            };
        }

        public override Type GetViewComponent()
        {
            return typeof(WriteLineComponent);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(WriteLineForm);
        }

        public override Type GetModelType()
        {
            return typeof(WriteLineModel);
        }
    }
}
