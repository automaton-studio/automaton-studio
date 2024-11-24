using Automaton.Core.Models;

namespace Automaton.Studio.Domain
{
    public class StringStepVariable : StepVariable
    {
        private string stringValue;

        public string StringValue
        {
            get => stringValue;
            set => stringValue = value;
        }

        public override object Value
        {
            get => StringValue;
            set => StringValue = (string)value;
        }
    }
}
