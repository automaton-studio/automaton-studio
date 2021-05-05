using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface ITreeActivityViewModel
    {
        IList<ActivityTreeModel> TreeItems { get; set; }
        ActivityTreeModel SelectedActivity { get; set; }
        Task Initialize();
    }
}
