using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class ActivitiesTreeViewModel : ITreeActivityViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IActivityService activityService;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IList<ActivityModel>? activities;
        public IList<ActivityModel> TreeItems
        {
            get => activities;

            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public ActivitiesTreeViewModel(
            IActivityService activityService,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.activityService = activityService;
            TreeItems = new List<ActivityModel>();
        }

        #region Public Methods

        public async Task Initialize()
        {
            var elsaActivities = await activityService.List();
            var activityItems = mapper.Map<IEnumerable<Elsa.Metadata.ActivityDescriptor>, IList<ActivityModel>>(elsaActivities);
            var categoryNames = activityItems.Select(x => x.Category).Distinct();

            foreach (var categoryName in categoryNames)
            {
                // Create category
                var category = new ActivityModel
                {
                    DisplayName = categoryName,
                    Activities = new List<ActivityModel>()
                };

                // Prepare category activities
                var categoryActivities = activityItems.Where(x => x.Category == categoryName);
                category.Activities.AddRange(categoryActivities);

                // Add category to the tree
                TreeItems.Add(category);
            }
        }

        #endregion

        #region Private Methods

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
