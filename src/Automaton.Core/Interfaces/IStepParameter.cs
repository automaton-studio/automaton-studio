namespace Automaton.Core.Interfaces
{
    public interface IStepParameter
    {
        void AssignInput(object data, IStepBody body, IStepExecutionContext context);
        void AssignOutput(object data, IStepBody body, IStepExecutionContext context);
    }
}