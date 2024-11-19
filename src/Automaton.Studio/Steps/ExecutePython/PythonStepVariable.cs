using Automaton.Core.Models;

namespace Automaton.Studio.Steps.ExecutePython
{
    public class PythonStepVariable : StepVariable
    {
        private string _pythonValue;

        public string PythonValue
        {
            get => _pythonValue;
            set => _pythonValue = value;
        }

        public override object Value
        {            
            get => PythonValue;
            set => PythonValue = (string)value;
        }
    }
}
