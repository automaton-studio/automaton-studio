using Automaton.Core.Models;
using Automaton.WebApi.Models;

namespace Automaton.WebApi.Interfaces
{
    public interface IDefinitionLoader
    {
        WorkflowDefinition LoadDefinition(Definition source);
        WorkflowDefinition LoadDefinition(string source, Func<string, Definition> deserializer);
    }
}