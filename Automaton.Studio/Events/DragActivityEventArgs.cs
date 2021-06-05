using Automaton.Studio.Activity;
using System;

namespace Automaton.Studio.Events
{
    public class DragActivityEventArgs : EventArgs
    {
        public DragActivityEventArgs(StudioActivity activity)
        {
            Activity = activity;
        }

        public StudioActivity? Activity { get; set; }
    }
}
