using Automaton.Core.Models;
using Automaton.Steps;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using System.Reflection;

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

        public SequenceEndStep SequenceEndStep { get; set; }

        public SequenceStep(StepFactory stepFactory)
        {
            this.stepFactory = stepFactory;

            Finalize += OnFinalize;
            Dropped += OnItemDropped;
        }

        public override Type GetDesignerComponent()
        {
            return typeof(SequenceDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }

        private void OnFinalize(object sender, StepEventArgs e)
        {
            var sequenceEndStep = CreateSequenceEndStep();
            AddSequenceEndStep(sequenceEndStep);
        }

        private void OnItemDropped(object sender, StepEventArgs e)
        {
            //var startIndex = Definition.Steps.IndexOf(this);
            //var endIndex = Definition.Steps.IndexOf(SequenceEndStep);
            //var steps = Definition.Steps.GetRange(startIndex, endIndex - startIndex);

            //Definition.Steps.RemoveRange(startIndex, endIndex - startIndex);
            //Definition.Steps.InsertRange(e.NewIndex, steps);
        }

        private void AddSequenceEndStep(SequenceEndStep sequenceEndStep)
        {
            SequenceEndStep = sequenceEndStep;
            Definition.Steps.Insert(Definition.Steps.IndexOf(this) + 1, sequenceEndStep);
            Definition.FinalizeStep(sequenceEndStep);
        }

        private SequenceEndStep CreateSequenceEndStep()
        {
            var stepDescription = typeof(SequenceEndStep).GetCustomAttribute<StepDescriptionAttribute>(false);
            var sequenceEndStep = stepFactory.CreateStep(stepDescription?.Name) as SequenceEndStep;
            sequenceEndStep.Definition = Definition;
            sequenceEndStep.SequencedStep = this;

            return sequenceEndStep;
        }
    }
}
