using Automaton.Core.Models;
using Automaton.Steps;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;

namespace Automaton.Studio.Steps.Sequence
{

    [StepDescription(
        Name = "Sequence",
        Type = "Sequence",
        DisplayName = "Sequence",
        Category = "Sequence",
        Description = "Sequence",
        Icon = "code"
    )]
    public class SequenceStep : StudioStep
    {
        private readonly StepFactory stepFactory;

        public override bool HasProperties { get; set; } = false;

        public SequenceStep(StepFactory stepFactory)
        {
            this.stepFactory = stepFactory;

            Finalize += OnStepFinalize;
        }

        public override Type GetDesignerComponent()
        {
            return typeof(SequenceDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }

        private void OnStepFinalize(object sender, StepEventArgs e)
        {
            var step = stepFactory.CreateStep("SequenceEnd");
            step.Definition = e.Step.Definition;

            var index = step.Definition.Steps.IndexOf(e.Step);

            step.Definition.Steps.Insert(index + 1, step);
            step.Definition.FinalizeStep(step);
        }
    }
}
