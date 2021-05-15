using Automaton.Activity;
using System;

namespace Automaton.Studio.Events
{
    public class ActivityChangedEventArgs : EventArgs
    {
        public ActivityChangedEventArgs(DynamicActivity dynamicActivity)
        {
            Activity = dynamicActivity;
        }

        public DynamicActivity? Activity { get; set; }
    }
}
