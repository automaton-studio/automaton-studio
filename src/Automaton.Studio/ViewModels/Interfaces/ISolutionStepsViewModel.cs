using AntDesign;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Automaton.Studio.ViewModels
{
    public interface ISolutionStepsViewModel
    {
        #region Events

        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        IEnumerable<SolutionStep> Steps { get; set; }

        #endregion

        #region Methods

        void Initialize();

        void StepDrag(TreeEventArgs<SolutionStep> solutionStep);

        #endregion
    }
}
