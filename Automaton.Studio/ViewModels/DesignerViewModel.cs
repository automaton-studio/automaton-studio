using AutoMapper;
using Automaton.Studio.Activities;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DesignerViewModel : IDesignerViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly DynamicActivityFactory activityFactory;
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;
        private readonly IRunnerService runnerService;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private WorkflowModel? workflow;
        public WorkflowModel Workflow
        {
            get => workflow;

            set
            {
                workflow = value;
                OnPropertyChanged();
            }
        }

        private IList<DynamicActivity> activities;
        public IList<DynamicActivity> Activities
        {
            get => activities;

            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public DesignerViewModel
        (
            DynamicActivityFactory activityFactory,
            IRunnerService runnerService,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IMapper mapper
        )
        {
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.mapper = mapper;
            this.workflowDefinitionStore = workflowDefinitionStore;

            Activities = new List<DynamicActivity>();

            Workflow = new()
            {
                Name = "Untitled",
                DisplayName = "Untitled",
                Version = 1
            };
        }

        public async Task LoadWorkflow(string workflow)
        {
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflow));
            Workflow = mapper.Map<WorkflowDefinition, WorkflowModel>(workflowDefinition);
            Activities = new List<DynamicActivity>();

            foreach (var activityDefinition in Workflow.Activities)
            {
                Activities.Add(activityFactory.GetDynamicActivity(activityDefinition));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
