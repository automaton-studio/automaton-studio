using Automaton.Core.Models;
using Automaton.WebApi.Models;

namespace Automaton.WebApi.Interfaces
{
    public interface IFlowLoader
    {
        Workflow LoadFlow(Flow source);
        Workflow LoadFlow(string source, Func<string, Flow> deserializer);
    }
}