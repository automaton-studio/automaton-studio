using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface ITreeActivityViewModel
    {
        #region Events

        event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Properties

        IList<TreeActivityModel> TreeItems { get; set; }
        TreeActivityModel SelectedActivity { get; set; }

        #endregion

        #region Methods

        void Initialize();
        void DragActivity(TreeActivityModel activityModel);

        #endregion
    }
}
