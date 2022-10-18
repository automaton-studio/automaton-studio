using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Pages.Designer.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceStepDesigner : ComponentBase
{
    private Dropzone dropzone;

    [Parameter]
    public StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    [Inject] private SequenceStepDesignerViewModel DesignerViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Setup event handlers after flow is loaded
        DesignerViewModel.DragStep += OnDragStep;
        DesignerViewModel.StepAdded += OnStepAdded;
        DesignerViewModel.StepRemoved += OnStepRemoved;
    }

    private void OnDropzoneMouseDown()
    {
        UnselectSteps();
    }

    private void OnStepAdded(object sender, StepEventArgs e)
    {
        StateHasChanged();
    }

    private void OnStepRemoved(object sender, StepEventArgs e)
    {
        StateHasChanged();
    }

    private void UnselectSteps()
    {
        var selectedSteps = GetSelectedSteps();

        if (selectedSteps != null)
        {
            foreach (var selectedStep in selectedSteps)
            {
                selectedStep.Unselect();
            }
        }
    }

    private void OnDragStep(object sender, StepEventArgs e)
    {
        dropzone.ActiveItem = e.Step;

        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the step being dragged
        dropzone.ActiveItem.Select();
    }

    private async Task OnStepDrop(StudioStep step)
    {
        DesignerViewModel.UpdateStepConnections();

        await Task.CompletedTask;
    }

    private void OnStepMouseDown(StudioStep step)
    {
        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the one under the mouse cursor
        step.Select();
    }

    private static void OnDelete(StudioStep step)
    {
        step.Definition.DeleteStep(step);
    }

    public IEnumerable<StudioStep> GetSelectedSteps()
    {
        return DesignerViewModel.Steps.Where(x => x.IsSelected());
    }
}
