using Polly;

namespace Automaton.Runner.Connection
{
    public interface IConnectionPolicy
    {
        AsyncPolicy GetPolicy();
    }
}
