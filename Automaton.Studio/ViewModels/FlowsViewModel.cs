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
    public class FlowsViewModel : IFlowsViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IRunnerService runnerService;
        private IFlowService flowService;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IList<FlowModel> flows;
        public IList<FlowModel> Flows
        {
            get => flows;

            set
            {
                flows = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<RunnerModel> runners;
        public IEnumerable<RunnerModel> Runners
        {
            get => runners;

            set
            {
                runners = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public FlowsViewModel
        (
            IRunnerService runnerService,
            IFlowService flowService,
            IMapper mapper
        )
        {
            this.flowService = flowService;
            this.runnerService = runnerService;
            this.mapper = mapper;

            Flows = mapper.Map<IEnumerable<StudioFlow>, IList<FlowModel>>(flowService.List());
            Runners = mapper.Map<IEnumerable<Runner>, IEnumerable<RunnerModel>>(runnerService.List());
        }

        /// <summary>
        /// Creates a new flow
        /// </summary>
        public async Task<FlowModel> CreateFlow(string flowName)
        {
            var flow = await flowService.Create(flowName);
            var flowModel = mapper.Map<StudioFlow, FlowModel>(flow);

            Flows.Add(flowModel);

            return flowModel;
        }

        /// <summary>
        /// Deletes a flow
        /// </summary>
        /// <param name="flow">Flow Id to delete</param>
        public void DeleteFlow(Guid flowId)
        {
            // Delete flow from DB
            flowService.Delete(flowId);

            // Delete flow from UI
            var tableFlow = Flows.SingleOrDefault(x => x.Id == flowId);
            Flows.Remove(tableFlow);
        }

        /// <summary>
        /// Runs flow on its selected runners
        /// </summary>
        /// <param name="flow">Flow model to run</param>
        public async Task RunFlow(FlowModel flow)
        {
            await runnerService.RunWorkflow(flow.StartupWorkflowId, flow.RunnerIds);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
