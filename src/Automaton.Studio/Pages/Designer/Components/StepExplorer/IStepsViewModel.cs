using AntDesign;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.Pages.Designer.Components.StepExplorer
{
    public interface IStepsViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IEnumerable<StepExplorerModel> Steps { get; set; }

        #endregion

        #region Methods

        void Initialize();

        void StepDrag(TreeEventArgs<StepExplorerModel> solutionStep);

        #endregion
    }
}
