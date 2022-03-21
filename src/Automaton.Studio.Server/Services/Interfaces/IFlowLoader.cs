using Automaton.Core.Models;
using Automaton.Studio.Server.Models;

namespace Automaton.Studio.Server.Services.Interfaces
{
    public interface IFlowLoader
    {
        Workflow LoadFlow(Flow source);
        Workflow LoadFlow(string source, Func<string, Flow> deserializer);
    }
}