using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DesignerViewModel : IDesignerViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IMapper mapper;
        private readonly ActivityFactory activityFactory;
        private readonly IRunnerService runnerService;
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;

        #endregion

        #region Properties

        public WorkflowDefinition ElsaWorkflow { get; set; }

        private StudioWorkflow studioWorkflow = new();
        public StudioWorkflow StudioWorkflow
        {
            get => studioWorkflow;

            set
            {
                studioWorkflow = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<WorkflowRunner>? runners;
        public IEnumerable<WorkflowRunner> Runners
        {
            get => runners;

            set
            {
                runners = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Guid>? runnerIds = new List<Guid>();
        public IEnumerable<Guid>? RunnerIds
        {
            get => runnerIds;

            set
            {
                runnerIds = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<ActivityEventArgs> DragActivity;

        #endregion

        public DesignerViewModel
        (
            IMapper mapper,
            ActivityFactory activityFactory,
            IRunnerService runnerService,
            IWorkflowDefinitionStore workflowDefinitionStore
        )
        {
            this.mapper = mapper;
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.workflowDefinitionStore = workflowDefinitionStore;
        }

        public async Task Initialize(string workflowId)
        {
            // Initialize runners
            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());

            // Load workflow
            if (!string.IsNullOrEmpty(workflowId))
            {
                await LoadWorkflow(workflowId);
            }
        }

        #region Public Methods

        public void DragTreeActivity(TreeActivity treeActivity)
        {
            var studioActivity = activityFactory.GetStudioActivity(treeActivity.Name);
            studioActivity.StudioWorkflow = StudioWorkflow;
            studioActivity.PendingCreation = true;

            mapper.Map(treeActivity, studioActivity);

            DragActivity?.Invoke(this, new ActivityEventArgs(studioActivity)); 
        }

        /// <summary>
        /// Save workflow to database
        /// </summary>
        public async Task SaveWorkflow()
        {
            mapper.Map(StudioWorkflow, ElsaWorkflow);

            await workflowDefinitionStore.SaveAsync(ElsaWorkflow);
        }

        /// <summary>
        /// Run workflow on specified runners
        /// </summary>
        /// <param name="workflow"></param>
        public async Task RunWorkflow()
        {
            await runnerService.RunWorkflow(StudioWorkflow.DefinitionId, RunnerIds);
        }

        #endregion

        #region Private Methods

        private async Task LoadWorkflow(string workflowId)
        {
            // Find Elsa workflow based on workflow id
            ElsaWorkflow = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));
            // Map Elsa to Studio workflow
            StudioWorkflow = mapper.Map<WorkflowDefinition, StudioWorkflow>(ElsaWorkflow);

            // We can't just map Elsa to Studio activities because of their different type 
            foreach (var activityDefinition in ElsaWorkflow.Activities)
            {
                var studioActivity = activityFactory.GetStudioActivity(activityDefinition);
                StudioWorkflow.AddActivity(studioActivity);
            }
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
