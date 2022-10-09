using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.Designer.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceStepDesigner : ComponentBase
{
    private Dropzone<StudioStep> dropzone;

    [Parameter]
    public Domain.StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    [Inject] 
    private ModalService ModalService { get; set; } = default!;

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

    private async Task OnStepDrop(Domain.StudioStep step)
    {
        if (!step.IsFinal())
        {
            await NewStepDialog(step);
        }
        else
        {
            DesignerViewModel.UpdateStepConnections();
        }
    }

    private void OnStepMouseDown(StudioStep step)
    {
        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the one under the mouse cursor
        step.Select();
    }

    private async Task OnStepDoubleClick(StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () =>
        {
            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private async Task OnEdit(StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () => {

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private static void OnDelete(StudioStep step)
    {
        step.Definition.DeleteStep(step);
    }

    private async Task NewStepDialog(StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () =>
        {
            DesignerViewModel.FinalizeStep(step);

            // TODO! It may be inneficient to update the state of the entire Designer control.
            // A better alternative would be to update the state of the step being updated.
            StateHasChanged();

            return Task.CompletedTask;
        };

        result.OnCancel = () =>
        {
            DesignerViewModel.DeleteStep(step);

            // TODO! It may be inneficient to update the state of the entire Designer control.
            // A better alternative would be to update the state of the activity being updated.
            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    public IEnumerable<StudioStep> GetSelectedSteps()
    {
        return DesignerViewModel.Steps.Where(x => x.IsSelected());
    }
}
