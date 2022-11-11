using Automaton.Studio.Domain;

namespace Automaton.Studio.Services;

public class DragDropService
{
    /// <summary>
    /// Steps being relocated around
    /// </summary>
    public IList<StudioStep> ActiveSteps { get; set; } = new List<StudioStep>();

    /// <summary>
    /// The item the active item is hovering above.
    /// </summary>
    public StudioStep DragTargetStep { get; set; }

    /// <summary>
    /// Holds the id of the Active Spacing div
    /// </summary>
    public int? ActiveSpacerId { get; set; }

    // Notify subscribers that there is a need for rerender
    public EventHandler StateHasChanged { get; set; }

    /// <summary>
    /// Resets the service to initial state
    /// </summary>
    public void Reset()
    {
        ActiveSteps.Clear();
        ActiveSpacerId = null;
        DragTargetStep = default;

        StateHasChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool HasActiveStep(string stepId)
    {
        return ActiveSteps.Select(x => x.Id).Contains(stepId);
    }
}
