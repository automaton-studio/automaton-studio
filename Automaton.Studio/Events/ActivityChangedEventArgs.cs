using Automaton.Studio.Activity;
using System;

namespace Automaton.Studio.Events
{
    public class ActivityChangedEventArgs : EventArgs
    {
        public ActivityChangedEventArgs(StudioActivity activity)
        {
            Activity = activity;
        }

        public StudioActivity? Activity { get; set; }
    }
}
