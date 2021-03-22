using System;

namespace Automaton.Studio.Activities
{
    public class ConsoleActivity : ActivityBase
    {
        public override Type GetViewComponent()
        {
            return typeof(ConsoleActivityComponent);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }
    }
}
