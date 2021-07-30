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
    public class TreeActivityViewModel : ITreeActivityViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IDesignerViewModel designerViewModel;
        private readonly ActivityFactory activityFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IList<TreeActivity> activities;
        public IList<TreeActivity> TreeItems
        {
            get => activities;

            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public TreeActivityViewModel(
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
            var activityItems = mapper.Map<IEnumerable<ActivityDescriptor>, IList<TreeActivity>>(activityDescriptors);
            var categoryNames = activityItems.Select(x => x.Category).Distinct();

            TreeItems = new List<TreeActivity>();

            foreach (var categoryName in categoryNames)
            {
                // Create category
                var category = new TreeActivity
                {
                    DisplayName = categoryName,
                    Activities = new List<TreeActivity>()
                };

                // Prepare category activities
                var categoryActivities = activityItems.Where(x => x.Category == categoryName);
                category.Activities.AddRange(categoryActivities);

                // Add category to the tree
                TreeItems.Add(category);
            }
        }

        public void DragActivity(TreeActivity activityModel)
        {
            if (!activityModel.IsCategory())
            {
                designerViewModel.DragTreeActivity(activityModel);
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
