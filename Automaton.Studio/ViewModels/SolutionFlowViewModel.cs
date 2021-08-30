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
    public class SolutionFlowViewModel : ISolutionFlowViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly ActivityFactory activityFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IList<WorkflowModel> treeWorkflows;
        public IList<WorkflowModel> Workflows
        {
            get => treeWorkflows;

            set
            {
                treeWorkflows = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SolutionFlowViewModel
        (
            ActivityFactory activityFactory,
            IMapper mapper
        )
        {
            this.mapper = mapper;
            this.activityFactory = activityFactory;
        }

        #region Public Methods

        public void Initialize()
        {
            var activityDescriptors = activityFactory.GetActivityDescriptors();
            var activityItems = mapper.Map<IEnumerable<ActivityDescriptor>, IList<ActivityModel>>(activityDescriptors);
            var categoryNames = activityItems.Select(x => x.Category).Distinct();

            Workflows = new List<WorkflowModel>();

            // TODO: Init list of workflows
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
