using Automaton.Studio.Core;
using Automaton.Studio.Core.Extensions;
using Automaton.Studio.Core.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Dialogs.MessageBox
{
    [StudioActivity(
        Name = "MessageBox",
        Type = "MessageBox",
        DisplayName = "Message Box",
        Category = "Dialogs",
        Description = "Write text to MessageBox",
        Icon = "info-circle"
    )]
    public class MessageBoxActivity : StudioActivity
    {
        #region Private Properties

        private ActivityDefinitionProperty TextProperty => GetProperty(nameof(Text));
        private ActivityDefinitionProperty CaptionProperty => GetProperty(nameof(Caption));

        #endregion

        #region Public Properties

        public string Text
        {
            get => TextProperty.GetExpression();

            set
            {
                TextProperty.SetExpression(value);
                OnPropertyChanged();
            }
        }

        public string Caption
        {
            get => CaptionProperty.GetExpression();

            set
            {
                CaptionProperty.SetExpression(value);
                OnPropertyChanged();
            }
        }

        #endregion

        public MessageBoxActivity(IActivityTypeDescriber activityDescriber) 
            : base(activityDescriber)
        {
            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Text), string.Empty),
                ActivityDefinitionProperty.Liquid(nameof(Caption), string.Empty),
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
