using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.ViewModels
{
    public interface ITreeFlowViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IList<TreeWorkflow> TreeWorkflows { get; set; }

        #endregion

        #region Methods

        void Initialize();

        #endregion
    }
}
