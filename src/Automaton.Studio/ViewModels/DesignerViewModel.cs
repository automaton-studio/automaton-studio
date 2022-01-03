using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Hosting;
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
        private readonly StepFactory stepFactory;
        private readonly IDefinitionService workflowService;

        #endregion

        #region Properties

        /// <summary>
        /// Studio flow
        /// </summary>
        private Definition studioFlow = new();
        public Definition StudioFlow
        {
            get => studioFlow;

            set
            {
                studioFlow = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<StepEventArgs> DragActivity;
        public event EventHandler<StepEventArgs> ActivityAdded
        {
            add
            {
                StudioFlow.ActivityAdded += value;
            }
            remove
            {

                StudioFlow.ActivityAdded -= value;
            }
        }

        public event EventHandler<StepEventArgs> ActivityRemoved
        {
            add
            {
                StudioFlow.ActivityRemoved += value;
            }
            remove
            {

                StudioFlow.ActivityRemoved -= value;
            }
        }

        #endregion

        #region Constructors

        public DesignerViewModel
        (
            IMapper mapper,
            StepFactory stepFactory,
            IDefinitionService workflowService
        )
        {
            this.mapper = mapper;
            this.stepFactory = stepFactory;
            this.workflowService = workflowService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Drag activity to the workflow.
        /// </summary>
        /// <param name="treeActivity"></param>
        public void ActivityDrag(SolutionStep treeActivity)
        {
            var activity = CreateActivity(treeActivity.Name);

            DragActivity?.Invoke(this, new StepEventArgs(activity));
        }

        /// <summary>
        /// Creates a Studio activity
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <returns>Studio activity</returns>
        private Step CreateActivity(string activityName)
        {
            var activity = stepFactory.GetStep(activityName);

            // Activity isn't final until confirmed by user.
            activity.PendingCreation = true;
            // Set reference to StudioWorkflow
            activity.StudioWorkflow = StudioFlow;

            return activity;
        }

        /// <summary>
        /// Adds activity to workflow
        /// </summary>
        /// <param name="activity"></param>
        public void AddActivity(Step activity)
        {
            StudioFlow.Steps.Add(activity);
        }

        /// <summary>
        /// Deletes specified activity
        /// </summary>
        /// <param name="activity">Activity to delete</param>
        public void DeleteActivity(Step activity)
        {
            StudioFlow.Steps.Remove(activity); 
        }

        /// <summary>
        /// Loads flow from database
        /// </summary>
        /// <param name="flowId">Flow identifier as string</param>
        public async Task LoadFlow(string flowId)
        {
            StudioFlow = await workflowService.Get(flowId);
        }

        /// <summary>
        /// Save flow to database
        /// </summary>
        public async Task SaveFlow()
        {
            await workflowService.Update(StudioFlow);
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
