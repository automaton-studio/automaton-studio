﻿using AutoMapper;
using Automaton.Studio.Activities;
using Automaton.Studio.Hubs;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<WorkflowHub> workflowHubContext;
        private readonly IRunnerService runnerService;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private WorkflowDefinition WorkflowDefinition { get; set; } = new()
        {
            Name = "Untitled",
            DisplayName = "Untitled",
            Version = 1
        };

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
            IHubContext<WorkflowHub> workflowHubContext,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IMapper mapper
        )
        {
            this.activityFactory = activityFactory;
            this.runnerService = runnerService;
            this.workflowHubContext = workflowHubContext;
            this.mapper = mapper;
            this.workflowDefinitionStore = workflowDefinitionStore;
        }

        public async Task Initialize()
        {
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification("SampleWorkflow"));
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