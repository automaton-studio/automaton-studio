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

        /// <summary>
        /// Elsa workflow
        /// </summary>
        public WorkflowDefinition ElsaWorkflow { get; set; }

        /// <summary>
        /// Studio workflow
        /// </summary>
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

        /// <summary>
        /// List of all runners
        /// </summary>
        private IEnumerable<WorkflowRunner> runners;
        public IEnumerable<WorkflowRunner> Runners
        {
            get => runners;

            set
            {
                runners = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Runners to execute Designer workflow
        /// </summary>
        private IEnumerable<Guid> selectedRunnerIds = new List<Guid>();
        public IEnumerable<Guid> SelectedRunnerIds
        {
            get => selectedRunnerIds;

            set
            {
                selectedRunnerIds = value;
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

            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        #region Public Methods

        /// <summary>
        /// Drag activity to the workflow.
        /// Activity isn't final until confirmed by user from activity dialog
        /// </summary>
        /// <param name="treeActivity"></param>
        public void DragTreeActivity(TreeActivity treeActivity)
        {
            var studioActivity = activityFactory.GetStudioActivity(treeActivity.Name);

            studioWorkflow.PendingActivity(studioActivity);

            DragActivity?.Invoke(this, new ActivityEventArgs(studioActivity));
        }

        /// <summary>
        /// Finalize pending activity
        /// </summary>
        /// <param name="activity"></param>
        public void FinalizeActivity(StudioActivity activity)
        {
            StudioWorkflow.FinalizeActivity(activity);
        }

        /// <summary>
        /// Adds activity to workflow
        /// </summary>
        /// <param name="activity"></param>
        public void AddActivity(StudioActivity activity)
        {
            StudioWorkflow.AddActivity(activity);
        }

        /// <summary>
        /// Load workflow
        /// </summary>
        /// <param name="workflowId">Workflow identifier</param>
        public async Task LoadWorkflow(string workflowId)
        {
            // Find Elsa workflow based on workflow id
            ElsaWorkflow = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));

            // Map Elsa to Studio
            StudioWorkflow = mapper.Map<WorkflowDefinition, StudioWorkflow>(ElsaWorkflow);

            // Elsa to Studio activities easily be mapped, so we are doing it here
            foreach (var activityDefinition in ElsaWorkflow.Activities)
            {
                var studioActivity = activityFactory.GetStudioActivity(activityDefinition);
                StudioWorkflow.AddActivity(studioActivity);
            }
        }

        /// <summary>
        /// Save workflow to database
        /// </summary>
        public async Task SaveWorkflow()
        {
            ElsaWorkflow ??= new WorkflowDefinition();

            // Update ElsaWorkflow with details from StudioWorkflow
            mapper.Map(StudioWorkflow, ElsaWorkflow);

            await workflowDefinitionStore.SaveAsync(ElsaWorkflow);
        }

        /// <summary>
        /// Run workflow on selected runners
        /// </summary>
        /// <param name="workflow"></param>
        public async Task RunWorkflow()
        {
            await runnerService.RunWorkflow(StudioWorkflow.DefinitionId, SelectedRunnerIds);
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
