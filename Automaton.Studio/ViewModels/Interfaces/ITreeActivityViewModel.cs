using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface ITreeActivityViewModel
    {
        IList<ActivityTreeItem> TreeItems { get; set; }

        Task Initialize();
    }
}
