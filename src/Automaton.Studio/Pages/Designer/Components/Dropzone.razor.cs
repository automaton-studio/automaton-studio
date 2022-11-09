using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components;

public partial class Dropzone : ComponentBase
{
    #region Consts

    private const string ActiveStepSpacingClass = "step-active-spacing";
    private const string StepSpacingClass = "step-spacing";

    #endregion

    private int _stepMargin;

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

    [Parameter] public EventCallback<StudioStep> ItemDoubleClick { get; set; }

    [Parameter] public IList<StudioStep> Steps { get; set; }

    [Parameter] public RenderFragment<StudioStep> ChildContent { get; set; }

    [Parameter] public string Class { get; set; }

    [Parameter] public Func<StudioStep, string> ItemWrapperClass { get; set; }

    protected override void OnInitialized()
    {
        _stepMargin = 10;

        DragDropService.StateHasChanged += ForceRender;

        base.OnInitialized();
    }

    public string CheckIfDraggable(StudioStep step)
    {
        if (AllowsDrag == null)
            return string.Empty;
        if (step == null)
            return string.Empty;
        if (AllowsDrag(step))
            return string.Empty;
        return "plk-dd-noselect";
    }

    public string GetStepMargin()
    {
        return $"{_stepMargin}px";
    }

    public string CheckIfDragOperationIsInProgess()
    {
        var activeItems = DragDropService.ActiveSteps;
        return activeItems.Any() ? "plk-dd-inprogess" : string.Empty;
    }

    public void SetActiveStep(StudioStep step)
    {
        DragDropService.ActiveSteps = new List<StudioStep>{ step };

        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the step being dragged
        foreach (var activeItem in DragDropService.ActiveSteps)
        {
            activeItem.Select();
        }
    }

    public void OnSpacerDrop()
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeSteps = DragDropService.ActiveSteps;
        var newIndex = DragDropService.ActiveSpacerId ?? Steps.Count - 1;

        int oldIndex;

        foreach(var item in activeSteps)
        {
            oldIndex = Steps.IndexOf(item);

            if (oldIndex >= 0)
            {
                Steps.RemoveAt(oldIndex);

                // The actual index could have shifted due to the removal
                if (newIndex > oldIndex)
                    newIndex--;
            }
        }

        foreach (var item in activeSteps)
        {
            Steps.Insert(newIndex++, item);
            ItemDrop.InvokeAsync(item);
        }

        DragDropService.Reset();
    }

    private void UnselectSteps()
    {
        var selectedSteps = Steps.Where(x => x.IsSelected()); ;

        if (selectedSteps != null)
        {
            foreach (var selectedStep in selectedSteps)
            {
                selectedStep.Unselect();
            }
        }
    }

    private void OnSpacerDragEnter(StudioStep item = null)
    {
        DragDropService.ActiveSpacerId = item != null ? GetItemIndex(item) : 0;
    }

    private void OnSpacerDragLeave()
    {
        DragDropService.ActiveSpacerId = null;
    }

    private string GetSpacerClass(StudioStep item)
    {
        var index = item != null ? GetItemIndex(item) : 0;
        string spacerClass;

        if (DragDropService.ActiveSpacerId == index)
        {
            spacerClass = ActiveStepSpacingClass;
        }
        else
        {
            spacerClass = StepSpacingClass;
        }       

        return spacerClass;
    }

    private int GetItemIndex(StudioStep item)
    {
        return Steps.IndexOf(item) + 1;
    }

    public void OnStepDragEnd()
    {
        if (DragEnd != null)
        {
            DragEnd(DragDropService.ActiveSteps);
        }

        DragDropService.Reset();
    }
    
    public void OnStepDragEnter(StudioStep step)
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

    public async Task OnStepDragOver(MouseEventArgs e, StudioStep step)
    {
        var activeSteps = DragDropService.ActiveSteps;

        if (activeSteps.Any(x => x.Id == step.Id))
            return;

        var firstHalf = await DragOverFirstStepHalf(e, step);

        var index = GetItemIndex(step);

        DragDropService.ActiveSpacerId = firstHalf ? index - 1 : index;
    }

    public void OnStepDragLeave()
    {
        DragDropService.DragTargetStep = default;
        StateHasChanged();
    }

    public void OnStepDragStart(StudioStep item)
    {
        DragDropService.ActiveSpacerId = null;
        DragDropService.ActiveSteps = Steps.Where(x => x.IsSelected()).ToList();
        StateHasChanged();
    }   

    private string GetClassesForDraggable(StudioStep item)
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
        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the one under the mouse cursor
        step.Select();

        ItemMouseDown.InvokeAsync(step);
    }

    private void OnStepDoubleClick(StudioStep item)
    {
        ItemDoubleClick.InvokeAsync(item);
    }

    private void OnDropzoneDrop()
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeItems = DragDropService.ActiveSteps;

        // no direct drag target
        if (DragDropService.DragTargetStep == null) 
        {
            foreach(var activeItem in activeItems)
            {
                // if dragged to another dropzone
                if (!Steps.Contains(activeItem))
                {
                    //insert item to new zone
                    Steps.Insert(Steps.Count, activeItem);
                }
                else
                {
                    //insert item to new zone if not final
                    if(!activeItem.IsFinal())
                        Steps.Insert(Steps.Count, activeItem);
                }
            }
        }
        // we have a direct target
        else
        {
            foreach (var activeItem in activeItems)
            {
                // if dragged to another dropzone
                if (!Steps.Contains(activeItem))
                {
                    //swap target with active item
                    Swap(DragDropService.DragTargetStep, activeItem);
                }
                else
                {
                    // if dragged to the same dropzone
                    //swap target with active item
                    Swap(DragDropService.DragTargetStep, activeItem);
                }
            }
        }

        foreach (var activeItem in activeItems)
        {
            ItemDrop.InvokeAsync(activeItem);
        }

        DragDropService.Reset();
        StateHasChanged();
    }

    private void Swap(StudioStep draggedOverItem, StudioStep activeItem)
    {
        var indexDraggedOverItem = Steps.IndexOf(draggedOverItem);
        var indexActiveItem = Steps.IndexOf(activeItem);
        if (indexActiveItem == -1) // item is new to the dropzone
        {
            //insert into new zone
            Steps.Insert(indexDraggedOverItem + 1, activeItem);
        }
        else //no instant replace, just insert it after 
        {
            if (indexDraggedOverItem == indexActiveItem)
                return;
            var tmp = Steps[indexActiveItem];
            Steps.RemoveAt(indexActiveItem);
            Steps.Insert(indexDraggedOverItem, tmp);
        }
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

    private void IncreaseStepMargin()
    {
        _stepMargin += 20;
    }

    private void DecreaseStepMargin()
    {
        _stepMargin -= 20;
    }

    public void Dispose()
    {
        DragDropService.StateHasChanged -= ForceRender;
    }
}
