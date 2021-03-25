using Elsa.Activities.Console;
using Elsa.Services;
using System;

namespace Automaton.Studio.Activities
{
    public class WriteLineActivity : ActivityBase
    {
        readonly IActivity elsaActivity = new WriteLine();

        public override IActivity ElsaActivity => elsaActivity;

        public override Type GetViewComponent()
        {
            return typeof(WriteLineComponent);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }
    }
}
