using Automaton.Studio.Activities.Attributes;
using Automaton.Studio.Activity;
using Elsa.Models;
using System;
using System.Linq;

namespace Automaton.Studio.SampleActivity
{
    [Activity(
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

        public override IStudioActivityModel Model { get; set; } = new SampleModel();

        public override Type GetViewComponent()
        {
            return typeof(SampleActivityComponent);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }

        public override Type GetModelType()
        {
            return typeof(SampleModel);
        }
    }
}
