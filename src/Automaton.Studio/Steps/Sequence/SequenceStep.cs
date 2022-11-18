using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using System.Reflection;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "Sequence",
    Type = "Sequence",
    DisplayName = "Sequence",
    Category = "Sequence",
    Description = "Sequence",
    Icon = "block"
)]
public class SequenceStep : StudioStep
{
    private readonly StepFactory stepFactory;

    public bool Collapsed { get; set; }

    public override bool HasProperties { get; set; } = false;

    public SequenceEndStep SequenceEndStep 
    {
        get
        {
            var sequenceEnd = Definition.Steps.SingleOrDefault(x => x.Id == SequenceEndStepId);
            return sequenceEnd as SequenceEndStep;
        }
    }

    public string SequenceEndStepId
    {
        get
        {
            return Inputs.ContainsKey(nameof(SequenceEndStepId)) ?
                Inputs[nameof(SequenceEndStepId)].ToString() : string.Empty;
        }

        set
        {
            Inputs[nameof(SequenceEndStepId)] = value;
        }
    }

    public IList<StudioStep> SequenceSteps { get; set; } = new List<StudioStep>();

    public SequenceStep(StepFactory stepFactory)
    {
        this.stepFactory = stepFactory;

        Finalize += OnFinalize;
    }

    public override void Setup(IStepDescriptor descriptor)
    {
        base.Setup(descriptor);

        SetupStepClass();
    }

    public override Type GetDesignerComponent()
    {
        return typeof(SequenceDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }

    public override void Select()
    {
        base.SetSelectClass();

        if (!IsFinal())
            return;

        var childrenAndEndStep = GetChildrenAndEndStep();

        foreach (var child in childrenAndEndStep)
        {
            child.SetSelectClass();
        }
    }

    public IEnumerable<StudioStep> GetChildren()
    {
        var children = Definition.Steps.Where(x => x.ParentId == Id);

        return children;
    }

    public IEnumerable<StudioStep> GetChildrenAndEndStep()
    {
        var sequenceEnd = Definition.Steps.SingleOrDefault(x => x.Id == SequenceEndStepId);

        var childrenAndEndStep = Definition.Steps.Where(x => x.ParentId == Id).ToList();

        childrenAndEndStep.Add(sequenceEnd);

        return childrenAndEndStep;
    }

    /// <summary>
    /// When finalized, each sequence must have a corresponding end sequence and
    /// they must kno about each other.
    /// </summary>
    private void OnFinalize(object sender, StepEventArgs e)
    {
        var sequenceEndStep = CreateSequenceEndStep();

        // Start and end sequences must know about each other
        SequenceEndStepId = sequenceEndStep.Id;

        // Add sequence end to definition's list of steps
        Definition.Steps.Insert(Definition.Steps.IndexOf(this) + 1, sequenceEndStep);

        // Ask definition to finalize sequence end
        Definition.FinalizeStep(sequenceEndStep);
    }

    /// <summary>
    /// Create a sequence end.
    /// </summary>
    /// <returns></returns>
    private SequenceEndStep CreateSequenceEndStep()
    {
        var stepDescription = typeof(SequenceEndStep).GetCustomAttribute<StepDescriptionAttribute>(false);
        
        var sequenceEndStep = stepFactory.CreateStep(stepDescription?.Name) as SequenceEndStep;
        sequenceEndStep.Definition = Definition;
        sequenceEndStep.SequenceStepId = Id;
        sequenceEndStep.ParentId = ParentId;

        return sequenceEndStep;
    }

    private void SetupStepClass()
    {
        StepClass = "designer-sequence-step";
        SelectedStepClass = "designer-sequence-step-selected";
        DisabledStepClass = "designer-sequence-step-disabled";

        Class = StepClass;
    }
}
