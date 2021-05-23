﻿using Automaton.Studio.Activities.Attributes;
using Automaton.Studio.Activity;
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
    public class WriteLineActivity : DynamicActivity
    {
        private ActivityDefinitionProperty TextProperty => GetDefinitionProperty(nameof(Text));
        public string Text => TextProperty.Expressions[TextProperty.Syntax];
        
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
            throw new NotImplementedException();
        }
    }
}