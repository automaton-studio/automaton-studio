using Automaton.Core.Models;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowConvertService
    {
        public Workflow ConvertFlow(Flow flow);
        public Flow ConvertFlow(Dto.Flow flowDto);
    }
}
