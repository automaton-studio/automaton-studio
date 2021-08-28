using AntDesign;
using AutoMapper;
using Automaton.Studio.Components;
using Automaton.Studio.Core;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
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

        private readonly NavigationManager navigationManager;
        private readonly ModalService modalService;
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
            NavigationManager navigationManager,
            ModalService modalService,
            IRunnerService runnerService,
            IFlowService flowService,
            MessageService messageService,
            IMapper mapper
        )
        {
            this.navigationManager = navigationManager;
            this.modalService = modalService;
            this.flowService = flowService;
            this.runnerService = runnerService;
            this.messageService = messageService;
            this.mapper = mapper;
        }

        public void Initialize()
        {
            var flows = flowService.List();
            Flows = mapper.Map<IEnumerable<StudioFlow>, IEnumerable<FlowModel>>(flows).ToList();
            Runners = mapper.Map<IQueryable<Runner>, IEnumerable<WorkflowRunner>>(runnerService.List());
        }

        public async Task CreateNewFlow()
        {
            var modalConfig = new ModalOptions
            {
                Title = Labels.NewFlowTitle,
                // Needed as a workaround to prevent dialog
                // close imediatelly when clicking OK button
                MaskClosable = false
            };

            var modalRef = await modalService.CreateModalAsync<NewFlow, FlowModel>(modalConfig, NewFlow);

            modalRef.OnOk = async () =>
            {
                // Needed to update OK button loading icon
                modalRef.Config.ConfirmLoading = true;
                await modalRef.UpdateConfigAsync();

                var studioFlow = await CreateFlow();

                navigationManager.NavigateTo($"designer/{studioFlow.Id}");
            };
        }

        private async Task<StudioFlow> CreateFlow()
        {
            try
            {
                var studioFlow = await flowService.Create(NewFlow.Name);

                return studioFlow;
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
