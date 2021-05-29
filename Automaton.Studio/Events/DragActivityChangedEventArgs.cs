using Automaton.Studio.Activity;
using System;

namespace Automaton.Studio.Events
{
    public class DragActivityChangedEventArgs : EventArgs
    {
        public DragActivityChangedEventArgs(StudioActivity activity)
        {
            Activity = activity;
        }

        public StudioActivity? Activity { get; set; }
    }
}
