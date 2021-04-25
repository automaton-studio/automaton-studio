using Automaton.Studio.Hubs;
using Automaton.Studio.Services;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class WorkflowsViewModel : IWorkflowsViewModel, INotifyPropertyChanged
    {
        #region Members

        private IWorkflowDefinitionService workflowService;
        private IRunnerService runnerService;
        private IHubContext<WorkflowHub> workflowHubContext;

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Properties

        private ICollection<Elsa.Client.Models.WorkflowDefinition>? workflows;
        public ICollection<Elsa.Client.Models.WorkflowDefinition> Workflows
        {
            get => workflows;

            set
            {
                workflows = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public WorkflowsViewModel(
            IWorkflowDefinitionService workflowService,
            IRunnerService runnerService,
            IHubContext<WorkflowHub> workflowHubContext)
        {
            this.workflowService = workflowService;
            this.runnerService = runnerService;
            this.workflowHubContext = workflowHubContext;
        }

        public async Task LoadWorkflows()
        {
            Workflows = (await workflowService.ListAsync()).Items;
        }

        public async Task RunWorkflow(string workflowId, string connectionId)
        {
            var client = workflowHubContext.Clients.Client(connectionId);

            await client.SendAsync("RunWorkflow", workflowId);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
