using AntDesign;
using AutoMapper;
using Automaton.Studio.Hubs;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class WorkflowsViewModel : IWorkflowsViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IWorkflowDefinitionStore workflowDefinitionStore;
        private readonly IHubContext<WorkflowHub> workflowHubContext;
        private readonly IRunnerService runnerService;
        private readonly MessageService messageService;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IEnumerable<WorkflowModel>? workflows;
        public IEnumerable<WorkflowModel> Workflows
        {
            get => workflows;

            set
            {
                workflows = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<RunnerModel>? runners;
        public IEnumerable<RunnerModel> Runners
        {
            get => runners;

            set
            {
                runners = value;
                OnPropertyChanged();
            }
        }

        public NewWorkflowModel NewWorkflowDetails { get; set; } = new NewWorkflowModel();

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public WorkflowsViewModel
        (
            IRunnerService runnerService,
            IHubContext<WorkflowHub> workflowHubContext,
            IWorkflowDefinitionStore workflowDefinitionStore,
            MessageService messageService,
            IMapper mapper
        )
        {
            this.runnerService = runnerService;
            this.messageService = messageService;
            this.workflowHubContext = workflowHubContext;
            this.mapper = mapper;
            this.workflowDefinitionStore = workflowDefinitionStore;
        }

        public async Task Initialize()
        {
            var workflowDefinitions = await workflowDefinitionStore.FindManyAsync(Specification<WorkflowDefinition>.Identity);
            Workflows = mapper.Map<IEnumerable<WorkflowDefinition>, IEnumerable<WorkflowModel>>(workflowDefinitions);     
            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<RunnerModel>>(runnerService.List());
        }

        public async Task RunWorkflow(WorkflowModel workflow)
        {
            if (!workflow.HasRunners)
                return;

            foreach (var runnerId in workflow.RunnerIds)
            {
                var runner = runnerService.Get(runnerId);
                var client = workflowHubContext.Clients.Client(runner.ConnectionId);
                await client.SendAsync("RunWorkflow", workflow.DefinitionId);
            }
        }

        public async Task NewWorkflow()
        {
            try
            {
                var workflowDefinition = mapper.Map<NewWorkflowModel, WorkflowDefinition>(NewWorkflowDetails);
                await workflowDefinitionStore.AddAsync(workflowDefinition);
            }
            catch (Exception ex)
            {
                await messageService.Error(Errors.NewWorkflowError);
                throw;
            }
            finally
            {
                ClearNewWorkflowDetails();
            }
        }

        private void ClearNewWorkflowDetails()
        {
            NewWorkflowDetails = new NewWorkflowModel();
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
