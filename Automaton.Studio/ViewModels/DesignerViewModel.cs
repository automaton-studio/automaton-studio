using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
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

        #endregion

        #region Properties

        /// <summary>
        /// Studio flow
        /// </summary>
        private StudioFlow studioFlow = new();
        public StudioFlow StudioFlow
        {
            get => studioFlow;

            set
            {
                studioFlow = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<StudioWorkflow> Workflows => StudioFlow.Workflows;

        public StudioWorkflow CurrentWorkflow => StudioFlow.CurrentWorkflow;

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

        #region Constructors

        public DesignerViewModel
        (
            IMapper mapper,
            IWebHostEnvironment env,
            ActivityFactory activityFactory,
            IRunnerService runnerService,
            IWorkflowService workflowService
        )
        {
            this.mapper = mapper;
            this.env = env;
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.workflowService = workflowService;

            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        #endregion

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

        /// <summary>
        /// Creates a Studio activity
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <returns>Studio activity</returns>
        private StudioActivity CreateActivity(string activityName)
        {
            var activity = activityFactory.GetStudioActivity(activityName);

            // Activity isn't final until confirmed by user.
            activity.PendingCreation = true;
            // Set reference to StudioWorkflow
            activity.StudioWorkflow = StudioFlow.CurrentWorkflow;

            return activity;
        }

        /// <summary>
        /// Finalize pending activity
        /// </summary>
        /// <param name="activity"></param>
        public void FinalizeActivity(StudioActivity activity)
        {
            StudioFlow.CurrentWorkflow.FinalizeActivity(activity);
        }

        /// <summary>
        /// Adds activity to workflow
        /// </summary>
        /// <param name="activity"></param>
        public void AddActivity(StudioActivity activity)
        {
            StudioFlow.CurrentWorkflow.AddActivity(activity);
        }

        /// <summary>
        /// Deletes specified activity
        /// </summary>
        /// <param name="activity">Activity to delete</param>
        public void DeleteActivity(StudioActivity activity)
        {
            StudioFlow.CurrentWorkflow.DeleteActivity(activity); 
        }

        /// <summary>
        /// Loads workflow from database
        /// </summary>
        /// <param name="workflowId">Workflow identifier</param>
        public async Task LoadWorkflow(string workflowId)
        {
            StudioFlow.CurrentWorkflow = await workflowService.LoadWorkflow(workflowId);
        }

        /// <summary>
        /// Save workflow to database
        /// </summary>
        public async Task SaveWorkflow()
        {
            await workflowService.SaveWorkflow(StudioFlow.CurrentWorkflow);
        }

        /// <summary>
        /// Run current workflow
        /// </summary>
        public async Task RunWorkflow()
        {
            if (env.IsDevelopment() && !SelectedRunnerIds.Any())
            {
                // Run on the server if in Development mode and there are no selected runners
                await workflowService.RunWorkflow(StudioFlow.CurrentWorkflow);
            }
            else
            {
                // Run workflow on specified runners if in Production
                await runnerService.RunWorkflow(StudioFlow.CurrentWorkflow.DefinitionId, SelectedRunnerIds);
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
