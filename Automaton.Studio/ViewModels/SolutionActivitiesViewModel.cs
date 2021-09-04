using AutoMapper;
using Automaton.Studio.Core.Metadata;
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
        private readonly ActivityFactory activityFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IList<ActivityModel> activities;
        public IList<ActivityModel> Activities
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
            ActivityFactory activityFactory,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.designerViewModel = designerViewModel;
            this.activityFactory = activityFactory;
        }

        #region Public Methods

        public void Initialize()
        {
            var activityDescriptors = activityFactory.GetActivityDescriptors();
            var activityItems = mapper.Map<IEnumerable<ActivityDescriptor>, IList<ActivityModel>>(activityDescriptors);
            var categoryNames = activityItems.Select(x => x.Category).Distinct();

            Activities = new List<ActivityModel>();

            foreach (var categoryName in categoryNames)
            {
                // Create category
                var category = new ActivityModel
                {
                    DisplayName = categoryName,
                    Activities = new List<ActivityModel>()
                };

                // Category activities
                var categoryActivities = activityItems.Where(x => x.Category == categoryName);
                category.Activities.AddRange(categoryActivities);

                // Add category to the tree
                Activities.Add(category);
            }
        }

        public void ActivityDrag(ActivityModel activityModel)
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
