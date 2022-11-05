using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using Automaton.Studio.Steps.Sequence;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components;

public partial class Dropzone : ComponentBase
{
    #region Consts

    private const string ActiveStepSpacingClass = "step-active-spacing";
    private const string StepSpacingClass = "step-spacing";

    #endregion

    [Inject] DragDropService DragDropService { get; set; }

    [Inject] JsInterop JsInterop { get; set; }

    [Parameter] public Func<IList<StudioStep>, StudioStep, bool> Accepts { get; set; }

    [Parameter] public Func<StudioStep, bool> AllowsDrag { get; set; }

    [Parameter] public Action<IList<StudioStep>> DragEnd { get; set; }

    [Parameter] public EventCallback OnDropzoneClick { get; set; }

    [Parameter] public EventCallback OnDropzoneMouseDown { get; set; }

    [Parameter] public EventCallback<IList<StudioStep>> OnItemDropRejected { get; set; }

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

    public IList<StudioStep> ActiveItems
    {
        get { return DragDropService.ActiveSteps; }
        set { DragDropService.ActiveSteps = value; }
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
        var activeItems = DragDropService.ActiveSteps;
        return activeItems.Any() ? "plk-dd-inprogess" : string.Empty;
    }

    public void SetActiveItem(StudioStep step)
    {
        ActiveItems = new List<StudioStep>{ step };

        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the step being dragged
        foreach (var activeItem in ActiveItems)
        {
            activeItem.Select();
        }
    }

    public void OnDropItemOnSpacing()
    {
        if (!IsDropAllowed())
        {
            DragDropService.Reset();
            return;
        }

        var activeItems = DragDropService.ActiveSteps;
        var newIndex = DragDropService.ActiveSpacerId ?? Items.Count - 1;

        int oldIndex;

        foreach(var item in activeItems)
        {
            oldIndex = Items.IndexOf(item);

            if (oldIndex >= 0)
            {
                Items.RemoveAt(oldIndex);

                // The actual index could have shifted due to the removal
                if (newIndex > oldIndex)
                    newIndex--;
            }
        }

        foreach (var item in activeItems)
        {
            Items.Insert(newIndex++, item);
            OnItemDrop.InvokeAsync(item);
        }

        DragDropService.Reset();
    }

    private void UnselectSteps()
    {
        var selectedSteps = Items.Where(x => x.IsSelected()); ;

        if (selectedSteps != null)
        {
            foreach (var selectedStep in selectedSteps)
            {
                selectedStep.Unselect();
            }
        }
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
            DragEnd(DragDropService.ActiveSteps);
        }

        DragDropService.Reset();
    }
    
    public void OnItemDragEnter(StudioStep step)
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

    public async Task OnItemDragOver(MouseEventArgs e, StudioStep step)
    {
        var activeSteps = DragDropService.ActiveSteps;

        if (activeSteps.Any(x => x.Id == step.Id))
            return;

        var firstHalf = await DragOverFirstStepHalf(e, step);

        var index = GetItemIndex(step);

        DragDropService.ActiveSpacerId = firstHalf ? index - 1 : index;
    }

    public void OnDragLeave()
    {
        DragDropService.DragTargetStep = default;
        StateHasChanged();
    }

    public void OnDragStart(StudioStep item)
    {
        DragDropService.ActiveSpacerId = null;
        DragDropService.ActiveSteps = Items.Where(x => x.IsSelected()).ToList();
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

    private void DropzoneClick()
    {
        OnDropzoneClick.InvokeAsync(null);
    }

    private void DropzoneMouseDown()
    {
        UnselectSteps();

        OnDropzoneMouseDown.InvokeAsync(null);
    }

    private void ItemClick(StudioStep item)
    {
        OnItemClick.InvokeAsync(item);
    }

    private void ItemMouseDown(StudioStep step)
    {
        // Unselect all the previous selected activities
        UnselectSteps();

        // Select the one under the mouse cursor
        step.Select();

        OnItemMouseDown.InvokeAsync(step);
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

        var activeItems = DragDropService.ActiveSteps;

        // no direct drag target
        if (DragDropService.DragTargetStep == null) 
        {
            foreach(var activeItem in activeItems)
            {
                // if dragged to another dropzone
                if (!Items.Contains(activeItem))
                {
                    //insert item to new zone
                    Items.Insert(Items.Count, activeItem);
                }
                else
                {
                    //insert item to new zone
                    Items.Insert(Items.Count, activeItem);
                }
            }
        }
        // we have a direct target
        else
        {
            foreach (var activeItem in activeItems)
            {
                // if dragged to another dropzone
                if (!Items.Contains(activeItem))
                {
                    if (!InstantReplace)
                    {
                        //swap target with active item
                        Swap(DragDropService.DragTargetStep, activeItem);
                    }
                }
                else
                {
                    // if dragged to the same dropzone
                    if (!InstantReplace)
                    {
                        //swap target with active item
                        Swap(DragDropService.DragTargetStep, activeItem);
                    }
                }
            }
        }

        foreach (var activeItem in activeItems)
        {
            OnItemDrop.InvokeAsync(activeItem);
        }

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

    public void Dispose()
    {
        DragDropService.StateHasChanged -= ForceRender;
    }
}
