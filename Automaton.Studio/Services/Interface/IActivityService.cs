using Elsa.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDescriptor>> List();
    }
}
