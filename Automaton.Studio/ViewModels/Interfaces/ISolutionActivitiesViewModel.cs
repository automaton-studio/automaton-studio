using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.ViewModels
{
    public interface ISolutionActivitiesViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IList<ActivityModel> TreeItems { get; set; }

        #endregion

        #region Methods

        void Initialize();

        void ActivityDrag(ActivityModel activityModel);

        #endregion
    }
}
