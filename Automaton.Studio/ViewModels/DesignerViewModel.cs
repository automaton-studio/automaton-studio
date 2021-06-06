using AutoMapper;
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

        private StudioWorkflow workflow = new();
        public StudioWorkflow Workflow
        {
            get => workflow;

            set
            {
                workflow = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<DragActivityEventArgs> DragActivity;

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
            Workflow = mapper.Map<WorkflowDefinition, StudioWorkflow>(workflowDefinition);

            foreach (var activityDefinition in workflowDefinition.Activities)
            {
                var studioActivity = activityFactory.GetStudioActivity(activityDefinition);      
                Workflow.Activities.Add(studioActivity);
            }
        }

        public void DragTreeActivity(TreeActivity treeActivity)
        {
            var studioActivity = activityFactory.GetStudioActivity(treeActivity.Name);
            studioActivity.PendingCreation = true;

            mapper.Map(treeActivity, studioActivity);

            DragActivity?.Invoke(this, new DragActivityEventArgs(studioActivity));
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
