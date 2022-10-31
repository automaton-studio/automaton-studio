using Automaton.Studio.Domain;

namespace Automaton.Studio.Events;

public class StepMovedEventArgs : EventArgs
{
    public StudioStep Step { get; set; }
    public int NewIndex { get; set; }

    public StepMovedEventArgs(StudioStep step, int newIndex)
    {
        Step = step;
        NewIndex = newIndex;
    }
}
