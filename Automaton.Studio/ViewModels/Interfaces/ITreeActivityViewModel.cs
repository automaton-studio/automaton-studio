using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.ViewModels
{
    public interface ITreeActivityViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IList<TreeActivity> TreeItems { get; set; }

        #endregion

        #region Methods

        void Initialize();

        void ActivityDrag(TreeActivity activityModel);

        #endregion
    }
}
