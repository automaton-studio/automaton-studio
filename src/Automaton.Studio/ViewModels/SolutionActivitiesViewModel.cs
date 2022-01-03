using AutoMapper;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.ViewModels
{
    public class SolutionActivitiesViewModel : ISolutionActivitiesViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IDesignerViewModel designerViewModel;
        private readonly StepFactory activityFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IEnumerable<SolutionStep> activities;
        public IEnumerable<SolutionStep> Activities
        {
            get => activities;

            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SolutionActivitiesViewModel(
            IDesignerViewModel designerViewModel,
            StepFactory activityFactory,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.designerViewModel = designerViewModel;
            this.activityFactory = activityFactory;
        }

        #region Public Methods

        public void Initialize()
        {
            Activities = activityFactory.GetSteps();
        }

        public void ActivityDrag(SolutionStep activityModel)
        {
            // Allow Drag only on activities, and not on categories
            if (!activityModel.IsCategory())
            {
                designerViewModel.ActivityDrag(activityModel);
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
