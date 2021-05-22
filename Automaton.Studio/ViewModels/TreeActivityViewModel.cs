using AutoMapper;
using Automaton.Studio.Activities.Factories;
using Automaton.Studio.Activity.Metadata;
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

        private IList<TreeActivityModel>? activities;
        public IList<TreeActivityModel> TreeItems
        {
            get => activities;

            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        public TreeActivityModel SelectedActivity { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

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
            var activityItems = mapper.Map<IEnumerable<ActivityDescriptor>, IList<TreeActivityModel>>(activityDescriptors);
            var categoryNames = activityItems.Select(x => x.Category).Distinct();

            TreeItems = new List<TreeActivityModel>();

            foreach (var categoryName in categoryNames)
            {
                // Create category
                var category = new TreeActivityModel
                {
                    DisplayName = categoryName,
                    Activities = new List<TreeActivityModel>()
                };

                // Prepare category activities
                var categoryActivities = activityItems.Where(x => x.Category == categoryName);
                category.Activities.AddRange(categoryActivities);

                // Add category to the tree
                TreeItems.Add(category);
            }
        }

        private void PopulateTreeItems()
        {
            
        }

        public void DragActivity(TreeActivityModel activityModel)
        {
            designerViewModel.DragActivity(activityModel);
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
