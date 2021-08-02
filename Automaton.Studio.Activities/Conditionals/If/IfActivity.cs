using Automaton.Studio.Core;
using Automaton.Studio.Core.Extensions;
using Automaton.Studio.Core.Metadata;
using Elsa;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Conditionals.If
{
    [StudioActivity(
        Name = "If",
        Type = "If",
        DisplayName = "If",
        Category = "Conditionals",
        Description = "If conditional",
        Icon = "apartment"
    )]
    public class IfActivity : StudioActivity
    {
        #region Private Properties

        private ActivityDefinitionProperty ConditionProperty => GetProperty(nameof(Condition));

        #endregion

        #region Public Properties

        public string Condition
        {
            get => ConditionProperty.GetExpression();

            set
            {
                ConditionProperty.SetExpression(value);
                OnPropertyChanged();
            }
        }

        #endregion

        public IfActivity(IActivityTypeDescriber activityDescriber) 
            : base(activityDescriber)
        {
            Properties = new List<ActivityDefinitionProperty>
            {
                ActivityDefinitionProperty.Liquid(nameof(Condition), string.Empty),
            };
        }

        public override Type GetDesignerComponent()
        {
            return typeof(IfDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(IfProperties);
        }

        public override void ConnectionAttached(StudioConnection connection)
        {
            base.ConnectionAttached(connection);

            connection.Outcome = OutcomeNames.True;
        }
    }
}
