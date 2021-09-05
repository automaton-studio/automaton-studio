using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class SolutionFlowViewModel : ISolutionFlowViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IMapper mapper;
        private readonly IFlowService flowService;

        #endregion

        #region Properties

        private IList<WorkflowModel> workflows;
        public IList<WorkflowModel> Workflows
        {
            get => workflows;

            set
            {
                workflows = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> WorkflowNames => Workflows.Select(x => x.Name);

        #endregion

        public SolutionFlowViewModel
        (
            IFlowService flowService,
            IMapper mapper
        )
        {
            this.mapper = mapper;
            this.flowService = flowService;
        }

        #region Public Methods

        public async Task LoadFlow(string flowId)
        {
            Guid.TryParse(flowId, out Guid flowIdGuid);
            var flow = await flowService.GetAsync(flowIdGuid);

            workflows = mapper.Map<IEnumerable<StudioWorkflow>, IList<WorkflowModel>>(flow.Workflows);
        }

        public void RenameWorkflow(string workflowId, string workflowName)
        {
            var workflow = Workflows.SingleOrDefault(x => x.Id == workflowId);
            workflow.Name = workflowName;
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
