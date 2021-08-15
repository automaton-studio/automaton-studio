using AntDesign;
using AutoMapper;
using Automaton.Studio.Entities;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Elsa.Persistence;
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
        private readonly MessageService messageService;
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

        public FlowModel NewFlow { get; set; } = new();

        #endregion

        public FlowsViewModel
        (
            IRunnerService runnerService,
            IFlowService flowService,
            MessageService messageService,
            IMapper mapper
        )
        {
            this.flowService = flowService;
            this.runnerService = runnerService;
            this.messageService = messageService;
            this.mapper = mapper;
        }

        public void Initialize()
        {            
            Flows = mapper.Map<IQueryable<Flow>, IEnumerable<FlowModel>>(flowService.List()).ToList();
            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        public async Task<FlowModel> CreateNewFlow()
        {
            try
            {
                var newFlow = mapper.Map<FlowModel, Flow>(NewFlow);
                await flowService.Create(NewFlow.Name);

                return mapper.Map<Flow, FlowModel>(newFlow);
            }
            catch
            {
                await messageService.Error(Errors.NewFlowError);
                throw;
            }
            finally
            {
                NewFlow.Reset();
            }
        }

        public void DeleteFlow(FlowModel flow)
        {
            // Delete flow entity
            flowService.Delete(flow.Id);

            // Delete flow from table
            var tableFlow = Flows.SingleOrDefault(x => x.Id == flow.Id);
            Flows.Remove(tableFlow);
        }

        public async Task RunWorkflow(FlowModel flow)
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
