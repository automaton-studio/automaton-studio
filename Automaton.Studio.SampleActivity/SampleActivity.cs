using Automaton.Studio.Activity;
using Elsa.Models;
using System;
using System.Linq;

namespace Automaton.Studio.SampleActivity
{
    [StudioActivity(
        Name = "SampleActivity",
        DisplayName = "SampleActivity",
        ElsaName = "ElsaSampleActivity",
        Category = "Sample",
        Description = "Sample activity that writes text to console"
    )]
    public class SampleActivity : StudioActivity
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

        public override Type GetDesignerComponent()
        {
            return typeof(SampleActivityDesigner);
        }

        public override Type GetDialogComponent()
        {
            throw new NotImplementedException();
        }
    }
}
