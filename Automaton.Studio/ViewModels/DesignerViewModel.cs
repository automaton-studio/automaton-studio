using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
        private readonly IWebHostEnvironment env;
        private readonly ActivityFactory activityFactory;
        private readonly IRunnerService runnerService;
        private readonly IWorkflowService workflowService;
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
            IWebHostEnvironment env,
            ActivityFactory activityFactory,
            IRunnerService runnerService,
            IWorkflowService workflowService,
            IWorkflowDefinitionStore workflowDefinitionStore
        )
        {
            this.mapper = mapper;
            this.env = env;
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.workflowService = workflowService;
            this.workflowDefinitionStore = workflowDefinitionStore;

            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        #region Public Methods

        /// <summary>
        /// Drag activity to the workflow.
        /// </summary>
        /// <param name="treeActivity"></param>
        public void ActivityDrag(TreeActivity treeActivity)
        {
            var activity = CreateActivity(treeActivity.Name);

            DragActivity?.Invoke(this, new ActivityEventArgs(activity));
        }

        private StudioActivity CreateActivity(string activityName)
        {
            var activity = activityFactory.GetStudioActivity(activityName);

            // Activity isn't final until confirmed by user.
            activity.PendingCreation = true;
            // Set reference to StudioWorkflow
            activity.StudioWorkflow = StudioWorkflow;

            return activity;
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
        /// Deletes specified activity
        /// </summary>
        /// <param name="activity">Activity to delete</param>
        public void DeleteActivity(StudioActivity activity)
        {
            StudioWorkflow.DeleteActivity(activity); 
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

            // Elsa to Studio activities are not easily mapped, so we are doing it here
            foreach (var activityDefinition in ElsaWorkflow.Activities)
            {
                var studioActivity = activityFactory.GetStudioActivity(activityDefinition);
                StudioWorkflow.LoadActivity(studioActivity);
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
        /// Run workflow
        /// </summary>
        public async Task RunWorkflow()
        {
            if (env.IsDevelopment() && !SelectedRunnerIds.Any())
            {
                // Update ElsaWorkflow with details from StudioWorkflow
                mapper.Map(StudioWorkflow, ElsaWorkflow);

                // Run on the server if in Development mode and there are no selected runners
                await workflowService.RunWorkflow(ElsaWorkflow);
            }
            else
            {
                // Run on specified runners if in Production
                await runnerService.RunWorkflow(StudioWorkflow.DefinitionId, SelectedRunnerIds);
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
