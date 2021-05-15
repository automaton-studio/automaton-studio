﻿using Elsa.Models;
using System;
using System.Linq;

namespace Automaton.Activity.WriteLine
{
    public class WriteLineActivity : DynamicActivity
    {
        private ActivityDefinitionProperty textProperty => Properties?.SingleOrDefault(x => x.Name == "Text");
        public string Text
        {
            get
            {
                return textProperty.Expressions[textProperty.Syntax];
            }

            set
            {
                textProperty.Expressions[textProperty.Syntax] = value;
            }
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