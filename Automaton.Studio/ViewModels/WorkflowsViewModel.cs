using AutoMapper;
using Automaton.Studio.Hubs;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using ElsaDashboard.Shared.Rpc;
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

        private readonly IWorkflowDefinitionService workflowService;
        private readonly IHubContext<WorkflowHub> workflowHubContext;
        private readonly IRunnerService runnerService;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public WorkflowsViewModel(
            IWorkflowDefinitionService workflowService,
            IRunnerService runnerService,
            IHubContext<WorkflowHub> workflowHubContext,
            IMapper mapper)
        {
            this.workflowService = workflowService;
            this.runnerService = runnerService;
            this.workflowHubContext = workflowHubContext;
            this.mapper = mapper;
        }

        public async Task Initialize()
        {
            var elsaWorkflows = (await workflowService.ListAsync()).Items;
            Workflows = mapper.Map<IEnumerable<Elsa.Client.Models.WorkflowDefinition>, IEnumerable<WorkflowModel>>(elsaWorkflows);     
            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<RunnerModel>>(runnerService.List());
        }

        public async Task RunWorkflow(WorkflowModel workflow)
        {
            if (workflow.Runners == null || !workflow.Runners.Any())
                return;

            foreach(var runnerId in workflow.Runners)
            {
                var runner = runnerService.Get(new Guid(runnerId));
                var client = workflowHubContext.Clients.Client(runner.ConnectionId);
                await client.SendAsync("RunWorkflow", workflow.DefinitionId);
            }

        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
