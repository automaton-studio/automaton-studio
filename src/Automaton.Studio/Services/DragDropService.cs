using Automaton.Studio.Domain;

namespace Automaton.Studio.Services;

public class DragDropService
{
    /// <summary>
    /// Currently Active Item
    /// </summary>
    public StudioStep ActiveStep { get; set; }

    /// <summary>
    /// The item the active item is hovering above.
    /// </summary>
    public StudioStep DragTargetStep { get; set; }

    /// <summary>
    /// Holds the id of the Active Spacing div
    /// </summary>
    public int? ActiveSpacerId { get; set; }

    /// <summary>
    /// Resets the service to initial state
    /// </summary>
    public void Reset()
    {
        ActiveStep = default;
        ActiveSpacerId = null;
        DragTargetStep = default;

        StateHasChanged?.Invoke(this, EventArgs.Empty);
    }

    // Notify subscribers that there is a need for rerender
    public EventHandler StateHasChanged { get; set; }
}
