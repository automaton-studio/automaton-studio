using Elsa.Models;

namespace Automaton.Studio.Core
{
    public class StudioConnection : ConnectionDefinition
    {
        public StudioConnection()
        {
        }

        public StudioConnection(StudioActivity sourceActivity, StudioActivity targetActivity, string outcome)
            : base(sourceActivity.ActivityId, targetActivity.ActivityId, outcome)
        {
            sourceActivity.ConnectionAttached(this);
        }
    }
}
