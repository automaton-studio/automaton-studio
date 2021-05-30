using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DesignerViewModel : IDesignerViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IMapper mapper;
        private readonly ActivityFactory activityFactory;
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;

        #endregion

        #region Properties

        private WorkflowModel workflow = new();
        public WorkflowModel Workflow
        {
            get => workflow;

            set
            {
                workflow = value;
                OnPropertyChanged();
            }
        }

        private StudioActivity selectedActivity;
        public StudioActivity SelectedActivity
        {
            get => selectedActivity;

            set
            {
                selectedActivity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<DragActivityChangedEventArgs> DragActivityChanged;

        #endregion


        public DesignerViewModel
        (
            IMapper mapper,
            ActivityFactory activityFactory,
            IWorkflowDefinitionStore workflowDefinitionStore
        )
        {
            this.mapper = mapper;
            this.activityFactory = activityFactory;
            this.workflowDefinitionStore = workflowDefinitionStore;
        }

        #region Public Methods

        public async Task LoadWorkflow(string workflowId)
        {
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));
            Workflow = mapper.Map<WorkflowDefinition, WorkflowModel>(workflowDefinition);

            foreach (var activityDefinition in workflowDefinition.Activities)
            {
                Workflow.Activities.Add(activityFactory.GetStudioActivity(activityDefinition));
            }
        }

        public void DragActivity(TreeActivityModel activityModel)
        {
            var activity = activityFactory.GetStudioActivity(activityModel.Name);

            mapper.Map(activityModel, activity);

            DragActivityChanged?.Invoke(this, new DragActivityChangedEventArgs(activity));
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
