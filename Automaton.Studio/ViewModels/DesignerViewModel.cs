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
        private readonly IFlowService flowService;

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

        /// <summary>
        /// Flow workflows
        /// </summary>
        public IList<StudioWorkflow> Workflows => StudioFlow.Workflows;

        /// <summary>
        /// Active workflow
        /// </summary>
        public StudioWorkflow ActiveWorkflow => StudioFlow.ActiveWorkflow;

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
            IWorkflowService workflowService,
            IFlowService flowService
        )
        {
            this.mapper = mapper;
            this.env = env;
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.workflowService = workflowService;
            this.flowService = flowService;

            Runners = mapper.Map<IEnumerable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Drag activity to the workflow.
        /// </summary>
        /// <param name="treeActivity"></param>
        public void ActivityDrag(ActivityModel treeActivity)
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
            activity.StudioWorkflow = StudioFlow.ActiveWorkflow;

            return activity;
        }

        /// <summary>
        /// Finalize pending activity
        /// </summary>
        /// <param name="activity"></param>
        public void FinalizeActivity(StudioActivity activity)
        {
            StudioFlow.ActiveWorkflow.FinalizeActivity(activity);
        }

        /// <summary>
        /// Adds activity to workflow
        /// </summary>
        /// <param name="activity"></param>
        public void AddActivity(StudioActivity activity)
        {
            StudioFlow.ActiveWorkflow.AddActivity(activity);
        }

        /// <summary>
        /// Deletes specified activity
        /// </summary>
        /// <param name="activity">Activity to delete</param>
        public void DeleteActivity(StudioActivity activity)
        {
            StudioFlow.ActiveWorkflow.DeleteActivity(activity); 
        }

        /// <summary>
        /// Loads flow from database
        /// </summary>
        /// <param name="flowId">Flow identifier</param>
        public async Task LoadFlow(Guid flowId)
        {
            StudioFlow = await flowService.GetAsync(flowId);
        }

        /// <summary>
        /// Loads flow from database
        /// </summary>
        /// <param name="flowId">Flow identifier as string</param>
        public async Task LoadFlow(string flowId)
        {
            Guid.TryParse(flowId, out Guid flowIdGuid);

            StudioFlow = await flowService.GetAsync(flowIdGuid);
        }

        /// <summary>
        /// Save flow to database
        /// </summary>
        public async Task SaveFlow()
        {
            await flowService.Update(StudioFlow);
        }

        /// <summary>
        /// Save workflow to database
        /// </summary>
        public async Task SaveWorkflow2()
        {
            await workflowService.SaveWorkflow(StudioFlow.ActiveWorkflow);
        }

        /// <summary>
        /// Run current workflow
        /// </summary>
        public async Task RunWorkflow()
        {
            if (env.IsDevelopment() && !SelectedRunnerIds.Any())
            {
                // Run on the server if in Development mode and there are no selected runners
                await workflowService.RunWorkflow(StudioFlow.ActiveWorkflow);
            }
            else
            {
                // Run workflow on specified runners if in Production
                await runnerService.RunWorkflow(StudioFlow.ActiveWorkflow.DefinitionId, SelectedRunnerIds);
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
