using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Dialogs.MessageBox
{
    [StudioActivity(
        Name = "MessageBoxActivity",
        DisplayName = "MessageBox",
        ElsaName = "MessageBox",
        Category = "Dialogs",
        Description = "Write text to MessageBox",
        Icon = "field-string"
    )]
    public class MessageBoxActivity : StudioActivity
    {
        public Automaton.MessageBox.MessageBox Activity { get; set; }

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

        public MessageBoxActivity(IActivityTypeDescriber activityDescriber) 
            : base(activityDescriber)
        {
            Name = "MessageBoxActivity";
            Type = "MessageBox";

            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Text), string.Empty)
            };
        }

        public override Type GetDesignerComponent()
        {
            return typeof(MessageBoxDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(MessageBoxProperties);
        }
    }
}
