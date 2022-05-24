using AntDesign;
using AutoMapper;
using Automaton.Studio.Factories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Pages.Designer.Components.StepExplorer
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        #region Members

        private readonly DesignerViewModel designerViewModel;
        private readonly StepFactory stepFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IEnumerable<StepExplorerModel> steps;
        public IEnumerable<StepExplorerModel> Steps
        {
            get => steps;

            set
            {
                steps = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public StepsViewModel(
            DesignerViewModel designerViewModel,
            StepFactory stepFactory,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.designerViewModel = designerViewModel;
            this.stepFactory = stepFactory;
        }

        #region Public Methods

        public void Initialize()
        {
            Steps = stepFactory.GetSteps();
        }

        public void StepDrag(TreeEventArgs<StepExplorerModel> args)
        {
            var step = args.Node.DataItem;

            if (!step.IsCategory())
            {
                designerViewModel.CreateStep(step);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
