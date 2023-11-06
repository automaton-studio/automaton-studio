using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace Automaton.Studio.Pages.FlowDesigner;

public partial class Designer : ComponentBase, IDisposable
{
    private const string ActiveStepSpacingClass = "step-active-spacing";
    private const string StepSpacingClass = "step-spacing";

    [Inject] KeyboardService KeyboardService { get; set; }
    [Inject] DragDropService DragDropService { get; set; }
    [Inject] JsInterop JsInterop { get; set; }

    [Parameter] public Func<IList<StudioStep>, StudioStep, bool> Accepts { get; set; }
    [Parameter] public Func<StudioStep, bool> AllowsDrag { get; set; }
    [Parameter] public Action<IList<StudioStep>> DragEnd { get; set; }
    [Parameter] public EventCallback DropzoneClick { get; set; }
    [Parameter] public EventCallback DropzoneMouseDown { get; set; }
    [Parameter] public EventCallback<IList<StudioStep>> OnItemDropRejected { get; set; }
    [Parameter] public EventCallback<StudioStep> ItemDrop { get; set; }
    [Parameter] public EventCallback<StudioStep> ItemClick { get; set; }
    [Parameter] public EventCallback<StudioStep> ItemMouseDown { get; set; }
    [Parameter] public EventCallback<StudioStep> ItemMouseUp { get; set; }
    [Parameter] public EventCallback<StudioStep> ItemDoubleClick { get; set; }
    [Parameter] public List<StudioStep> Steps { get; set; }
    [Parameter] public RenderFragment<StudioStep> ChildContent { get; set; }
    [Parameter] public string Class { get; set; }
    [Parameter] public Func<StudioStep, string> ItemWrapperClass { get; set; }

    public IList<StudioStep> VisibleSteps => Steps.Where(x => x.IsVisible()).ToList();

    protected override void OnInitialized()
    {
        DragDropService.StateHasChanged += ForceRender;

        base.OnInitialized();
    }

    public void SetActiveStep(StudioStep step)
    {
        DragDropService.ActiveSteps = new List<StudioStep> { step };
    }

    private void OnDrop()
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeSpacerIndex = DragDropService.ActiveSpacerId ?? Steps.Count;

        var firstActiveStep = DragDropService.ActiveSteps.First();
        var lastActiveStep = DragDropService.ActiveSteps.Last();

        var firstActiveStepIndex = Steps.IndexOf(firstActiveStep);
        var lastActiveStepIndex = Steps.IndexOf(lastActiveStep);

        // Is new step
        if (firstActiveStepIndex == -1 && lastActiveStepIndex == -1)
        {
            foreach (var step in DragDropService.ActiveSteps)
            {
                Steps.Insert(activeSpacerIndex, step);

                step.UpdateParent();
                step.UpdateVisibility();

                ItemDrop.InvokeAsync(step);
            }
        }
        // Are existing steps
        else if (activeSpacerIndex < firstActiveStepIndex || activeSpacerIndex > lastActiveStepIndex + 1)
        {
            var stepIndex = Steps.IndexOf(firstActiveStep);

            if (stepIndex >= 0)
            {
                Steps.RemoveRange(firstActiveStepIndex, DragDropService.ActiveSteps.Count);
            }

            if (lastActiveStepIndex >= 0 && activeSpacerIndex > lastActiveStepIndex) 
                activeSpacerIndex -= DragDropService.ActiveSteps.Count;

            foreach (var step in DragDropService.ActiveSteps)
            {                
                Steps.Insert(activeSpacerIndex++, step);

                step.UpdateParent();
                step.UpdateVisibility();

                ItemDrop.InvokeAsync(step);
            }
        }

        DragDropService.Reset();
    }

    private void OnStepDragEnd()
    {
        if (DragEnd != null)
        {
            DragEnd(DragDropService.ActiveSteps);
        }

        DragDropService.Reset();
    }

    private void OnStepDragEnter(StudioStep step)
    {
        var activeSteps = DragDropService.ActiveSteps;

        if (activeSteps.Any(x => x.Id == step.Id))
            return;

        if (!IsValidItem())
            return;

        if (!IsItemAccepted(step))
            return;

        DragDropService.DragTargetStep = step;

        StateHasChanged();
    }

    private async Task OnStepDragOver(MouseEventArgs e, StudioStep step)
    {
        var activeSteps = DragDropService.ActiveSteps;

        if (activeSteps.Any(x => x.Id == step.Id))
            return;

        var firstHalf = await DragOverFirstStepHalf(e, step);

        var index = Steps.IndexOf(step);

        DragDropService.ActiveSpacerId = firstHalf ? index : index + 1;
    }

    private void OnStepDragLeave()
    {
        DragDropService.DragTargetStep = default;

        StateHasChanged();
    }

    private void OnStepDragStart(StudioStep item)
    {
        DragDropService.ActiveSpacerId = null;
        DragDropService.ActiveSteps = Steps.Where(x => x.IsSelected()).ToList();

        StateHasChanged();
    }

    private void OnSpacerDragEnter(StudioStep item = null)
    {
        DragDropService.ActiveSpacerId = item != null ? Steps.IndexOf(item) + 1 : 0;
    }

    private void OnSpacerDragLeave()
    {
        DragDropService.ActiveSpacerId = null;
    }

    private void UnselectSteps()
    {
        var selectedSteps = Steps.Where(x => x.IsSelected());

        if (selectedSteps != null)
        {
            foreach (var selectedStep in selectedSteps)
            {
                selectedStep.Unselect();
            }
        }
    }

    private string GetSpacerClass(StudioStep item)
    {
        var index = item != null ? Steps.IndexOf(item) + 1 : 0;

        var spacerClass = DragDropService.ActiveSpacerId == index ? ActiveStepSpacingClass : StepSpacingClass;

        return spacerClass;
    }

    private string GetDraggableClass(StudioStep item)
    {
        var builder = new StringBuilder();
        builder.Append("plk-dd-draggable");

        if (ItemWrapperClass != null)
        {
            var itemWrapperClass = ItemWrapperClass(item);
            builder.AppendLine(" " + itemWrapperClass);
        }

        return builder.ToString();
    }

    private string GetClassesForDropzone()
    {
        var builder = new StringBuilder();
        builder.Append("plk-dd-dropzone");

        if (!string.IsNullOrEmpty(Class))
        {
            builder.AppendLine(" " + Class);
        }

        return builder.ToString();
    }

    private bool IsDropAllowed()
    {
        var activeItems = DragDropService.ActiveSteps;

        if (!IsValidItem())
        {
            return false;
        }

        if (!IsItemAccepted(DragDropService.DragTargetStep))
        {
            OnItemDropRejected.InvokeAsync(activeItems);
            return false;
        }

        return true;
    }

    private void OnDropzoneClick()
    {
        DropzoneClick.InvokeAsync(null);
    }

    private void OnDropzoneMouseDown()
    {
        UnselectSteps();

        DropzoneMouseDown.InvokeAsync(null);
    }

    private void OnStepClick(StudioStep item)
    {
        ItemClick.InvokeAsync(item);
    }

    private void OnStepMouseDown(StudioStep step)
    {
        SelectStep(step);

        ItemMouseDown.InvokeAsync(step);
    }

    private void OnStepMouseUp(StudioStep step)
    {
        if (!KeyboardService.ControlKeysDown())
        {
            UnselectSteps();
            step.Select();
        }

        ItemMouseUp.InvokeAsync(step);
    }

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "a" && KeyboardService.ControlDown())
        {
            foreach (var steps in Steps)
            {
                steps.Select();
            }
        }
    }

    public void SelectStep(StudioStep step)
    {
        if (KeyboardService.ControlDown())
        {
            if (step.IsSelected())
                step.Unselect();
            else
                step.Select();
        }
        else if (KeyboardService.ShiftDown())
        {
            step.Select();

            var selectedSteps = Steps.Where(x => x.IsSelected());
            var firstStep = selectedSteps.FirstOrDefault();
            var lastStep = selectedSteps.LastOrDefault();
            var firstStepIndex = Steps.IndexOf(firstStep);
            var lastStepIndex = Steps.IndexOf(lastStep);
            var stepsToSelect = Steps.GetRange(firstStepIndex, lastStepIndex - firstStepIndex);

            foreach(var stepToSelect in stepsToSelect)
            {
                stepToSelect.Select();
            }
        }
        else
        {
            var selectedSteps = Steps.Where(x => x.IsSelected());
            if (!selectedSteps.Contains(step))
                UnselectSteps();

            step.Select();
        }
    }

    public void CompleteStep(StudioStep step)
    {
        step.Complete();
        SelectStep(step);
    }

    private void OnStepDoubleClick(StudioStep item)
    {
        ItemDoubleClick.InvokeAsync(item);
    }

    private string IsStepDragable(StudioStep item)
    {
        if (AllowsDrag == null)
            return "true";

        if (item == null)
            return "false";

        return AllowsDrag(item).ToString();
    }

    private bool IsItemAccepted(StudioStep dragTargetItem)
    {
        if (Accepts == null)
            return true;
        return Accepts(DragDropService.ActiveSteps, dragTargetItem);
    }

    private bool IsValidItem()
    {
        return DragDropService.ActiveSteps.Any();
    }

    private void ForceRender(object sender, EventArgs e)
    {
        StateHasChanged();
    }

    private async Task<bool> DragOverFirstStepHalf(MouseEventArgs e, StudioStep step)
    {
        var boundingRect = await JsInterop.GetBoundingClientRect(step.Id);

        var firstHalf = e.ClientY >= boundingRect.Y && e.ClientY <= boundingRect.Y + boundingRect.Height / 2;

        return firstHalf;
    }

    private string GetNoDragableClass(StudioStep step)
    {
        if (AllowsDrag == null)
            return string.Empty;

        if (step == null)
            return string.Empty;

        if (AllowsDrag(step))
            return string.Empty;

        return "plk-dd-noselect";
    }

    private string GetStepVisibilityClass(StudioStep step)
    {
        return !step.IsVisible() ? "step-invisible" : string.Empty;
    }

    private string GetDragOperationClass()
    {
        var activeItems = DragDropService.ActiveSteps;
        return activeItems.Any() ? "plk-dd-inprogess" : string.Empty;
    }

    private int GetVisibleStepIndex(StudioStep step)
    {
        return VisibleSteps.IndexOf(step);
    }

    public void Dispose()
    {
        DragDropService.StateHasChanged -= ForceRender;
    }
}
