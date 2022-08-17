using Automaton.Studio.Domain;

namespace Automaton.Studio.Events;

public class StepEventArgs : EventArgs
{
    public StudioStep Step { get; set; }

    public StepEventArgs(StudioStep step)
    {
        Step = step;
    }
}
