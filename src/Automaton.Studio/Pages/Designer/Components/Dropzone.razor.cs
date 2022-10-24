using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using Automaton.Studio.Steps.Sequence;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Automaton.Studio.Pages.Designer.Components;

public partial class Dropzone : ComponentBase
{
    #region Consts

    private const string ActiveStepSpacingClass = "step-active-spacing";
    private const string StepSpacingClass = "step-spacing";

    #endregion

    [Inject] DragDropService DragDropService { get; set; }

    [Parameter] public Func<StudioStep, StudioStep, bool> Accepts { get; set; }

    [Parameter] public Func<StudioStep, bool> AllowsDrag { get; set; }

    [Parameter] public Action<StudioStep> DragEnd { get; set; }

    [Parameter] public EventCallback OnDropzoneClick { get; set; }

    [Parameter] public EventCallback OnDropzoneMouseDown { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemDropRejected { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemDrop { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemClick { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemMouseDown { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemDoubleClick { get; set; }

    [Parameter] public EventCallback<StudioStep> OnReplacedItemDrop { get; set; }

    [Parameter] public bool InstantReplace { get; set; }

    [Parameter] public IList<StudioStep> Items { get; set; }

    [Parameter] public int? MaxItems { get; set; }

    [Parameter] public EventCallback<StudioStep> OnItemDropRejectedByMaxItemLimit { get; set; }

    [Parameter] public RenderFragment<StudioStep> ChildContent { get; set; }

    [Parameter] public string Class { get; set; }

    [Parameter] public string Id { get; set; }

    [Parameter] public Func<StudioStep, string> ItemWrapperClass { get; set; }

    public StudioStep ActiveItem
    {
        get { return DragDropService.ActiveStep; }
        set { DragDropService.ActiveStep = value; }
    }

    protected override void OnInitialized()
    {
        Id = Guid.NewGuid().ToString();
        DragDropService.StateHasChanged += ForceRender;

        base.OnInitialized();
    }

    public string CheckIfDraggable(StudioStep item)
    {
        if (AllowsDrag == null)
            return string.Empty;
        if (item == null)
            return string.Empty;
        if (AllowsDrag(item))
            return string.Empty;
        return "plk-dd-noselect";
    }

    public string CheckIfDragOperationIsInProgess()
    {
        var activeItem = DragDropService.ActiveStep;
        return activeItem == null ? string.Empty : "plk-dd-inprogess";
    }

    public void OnDropItemOnSpacing(int newIndex)
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeItem = DragDropService.ActiveStep;
        var oldIndex = Items.IndexOf(activeItem);

        if (oldIndex == -1) // item not present in target dropzone
        {
            if (DragDropService.Items != null)
                DragDropService.Items.Remove(activeItem);
        }
        else // same dropzone drop
        {
            Items.RemoveAt(oldIndex);
            // the actual index could have shifted due to the removal
            if (newIndex > oldIndex)
                newIndex--;
        }

        Items.Insert(newIndex, activeItem);

        OnItemDrop.InvokeAsync(activeItem);

        //Operation is finished
        DragDropService.Reset();
    }

    private void OnDragEnterFirstSpacing()
    {
        DragDropService.ActiveSpacerId = 0;
    }

    private void OnDragLeaveFirstSpacing()
    {
        DragDropService.ActiveSpacerId = null;
    }

    private void OnDragEnterSpacing(StudioStep item)
    {
        DragDropService.ActiveSpacerId = GetItemIndex(item);
    }

    private void OnDragLeaveSpacing(StudioStep item)
    {
        DragDropService.ActiveSpacerId = null;
    }

    private void OnDragEnterDropzoneSpacing(DragEventArgs e)
    {
        DragDropService.ActiveSpacerId = Items.Count;
    }

    private void OnDragLeaveDropzoneSpacing()
    {
        DragDropService.ActiveSpacerId = null;
    }

    private string GetStepSpacerClass(StudioStep item)
    {
        var index = item != null ? GetItemIndex(item) : 0;
        var spacerClass = DragDropService.ActiveSpacerId == index ? ActiveStepSpacingClass : StepSpacingClass;

        return spacerClass;
    }

    private int GetItemIndex(StudioStep item)
    {
        return Items.IndexOf(item) + 1;
    }

    public void OnDragEnd()
    {
        if (DragEnd != null)
        {
            DragEnd(DragDropService.ActiveStep);
        }

        DragDropService.Reset();
    }
    
    public void OnItemDragOver(StudioStep step)
    {
        DragDropService.ActiveSpacerId = GetItemIndex(step);
    }

    public void OnItemDragEnter(StudioStep step)
    {
        var activeStep = DragDropService.ActiveStep;

        if (step.Equals(activeStep))
            return;

        if (!IsValidItem())
            return;

        if (IsMaxItemLimitReached())
            return;

        if (!IsItemAccepted(step))
            return;

        activeStep.Dropzone = this;
      
        DragDropService.DragTargetStep = step;

        if (InstantReplace && step is not SequenceStep)
        {
            Swap(DragDropService.DragTargetStep, activeStep);
        }

        StateHasChanged();
    }

    public void OnDragLeave()
    {
        DragDropService.DragTargetStep = default;
        StateHasChanged();
    }

    public void OnDragStart(StudioStep item)
    {
        DragDropService.ActiveSpacerId = null;
        DragDropService.ActiveStep = item;
        DragDropService.Items = Items;
        StateHasChanged();
    }   

    private string CheckIfItemIsInTransit(StudioStep item)
    {
        return item.Equals(DragDropService.ActiveStep) ? "plk-dd-in-transit no-pointer-events" : string.Empty;
    }

    private string CheckIfItemIsDragTarget(StudioStep item)
    {
        if (item.Equals(DragDropService.ActiveStep))
            return string.Empty;

        if (item.Equals(DragDropService.DragTargetStep))
        {
            return IsItemAccepted(DragDropService.DragTargetStep) ? "plk-dd-dragged-over" : "plk-dd-dragged-over-denied";
        }

        return string.Empty;
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
        var activeItem = DragDropService.ActiveStep;
        if (!IsValidItem())
        {
            return false;
        }

        if (IsMaxItemLimitReached())
        {
            OnItemDropRejectedByMaxItemLimit.InvokeAsync(activeItem);
            return false;
        }

        if (!IsItemAccepted(DragDropService.DragTargetStep))
        {
            OnItemDropRejected.InvokeAsync(activeItem);
            return false;
        }

        return true;
    }

    private void DropzoneClick()
    {
        OnDropzoneClick.InvokeAsync(null);
    }

    private void DropzoneMouseDown()
    {
        OnDropzoneMouseDown.InvokeAsync(null);
    }

    private void ItemClick(StudioStep item)
    {
        OnItemClick.InvokeAsync(item);
    }

    private void ItemMouseDown(StudioStep item)
    {
        OnItemMouseDown.InvokeAsync(item);
    }

    private void ItemDoubleClick(StudioStep item)
    {
        OnItemDoubleClick.InvokeAsync(item);
    }

    private void OnDrop()
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeItem = DragDropService.ActiveStep;

        if (DragDropService.DragTargetStep == null) //no direct drag target
        {
            if (!Items.Contains(activeItem)) //if dragged to another dropzone
            {
                Items.Insert(Items.Count, activeItem); //insert item to new zone
                if (DragDropService.Items != null)
                    DragDropService.Items.Remove(activeItem); //remove from old zone            
            }
            else
            {
                //what to do here?
            }
        }
        else // we have a direct target
        {
            if (!Items.Contains(activeItem)) // if dragged to another dropzone
            {
                if (!InstantReplace)
                {
                    Swap(DragDropService.DragTargetStep, activeItem); //swap target with active item
                }
            }
            else
            {
                // if dragged to the same dropzone
                if (!InstantReplace)
                {
                    Swap(DragDropService.DragTargetStep, activeItem); //swap target with active item
                }
            }
        }

        OnItemDrop.InvokeAsync(activeItem);

        DragDropService.Reset();
        StateHasChanged();
    }

    private void Swap(StudioStep draggedOverItem, StudioStep activeItem)
    {
        var indexDraggedOverItem = Items.IndexOf(draggedOverItem);
        var indexActiveItem = Items.IndexOf(activeItem);
        if (indexActiveItem == -1) // item is new to the dropzone
        {
            //insert into new zone
            Items.Insert(indexDraggedOverItem + 1, activeItem);
            //remove from old zone
            if (DragDropService.Items != null)
                DragDropService.Items.Remove(activeItem);
        }
        else if (InstantReplace) //swap the items
        {
            if (indexDraggedOverItem == indexActiveItem)
                return;
            StudioStep tmp = Items[indexDraggedOverItem];
            Items[indexDraggedOverItem] = Items[indexActiveItem];
            Items[indexActiveItem] = tmp;
            OnReplacedItemDrop.InvokeAsync(Items[indexActiveItem]);
        }
        else //no instant replace, just insert it after 
        {
            if (indexDraggedOverItem == indexActiveItem)
                return;
            var tmp = Items[indexActiveItem];
            Items.RemoveAt(indexActiveItem);
            Items.Insert(indexDraggedOverItem, tmp);
        }
    }

    private bool IsMaxItemLimitReached()
    {
        var activeItem = DragDropService.ActiveStep;
        return (!Items.Contains(activeItem) && MaxItems.HasValue && MaxItems == Items.Count());
    }

    private string IsItemDragable(StudioStep item)
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
        return Accepts(DragDropService.ActiveStep, dragTargetItem);
    }

    private bool IsValidItem()
    {
        return DragDropService.ActiveStep != null;
    }

    private void ForceRender(object sender, EventArgs e)
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        DragDropService.StateHasChanged -= ForceRender;
    }
}
