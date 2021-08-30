using AntDesign;
using AutoMapper;
using Automaton.Studio.Components;
using Automaton.Studio.Core;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
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

            Flows = mapper.Map<IEnumerable<StudioFlow>, IList<FlowModel>>(flowService.List());
            Runners = mapper.Map<IEnumerable<Runner>, IEnumerable<RunnerModel>>(runnerService.List());
        }

        /// <summary>
        /// Creates a new flow
        /// </summary>
        public async Task CreateFlow()
        {
            var modalConfig = new ModalOptions
            {
                Title = Labels.NewFlowTitle,
                // Observation:
                // Needed as a workaround to prevent dialog
                // close imediatelly when clicking OK button
                MaskClosable = false
            };

            // Open NewFlow component as a modal dialog.
            // When OK button is clicked, we create the flow.
            var flowModel = new FlowModel();
            var modalRef = await modalService.CreateModalAsync<NewFlow, FlowModel>(modalConfig, flowModel);

            modalRef.OnOk = async () =>
            {
                // Needed to update OK button loading icon
                modalRef.Config.ConfirmLoading = true;
                await modalRef.UpdateConfigAsync();

                try
                {
                    var studioFlow = await flowService.Create(flowModel.Name);
                    navigationManager.NavigateTo($"designer/{studioFlow.Id}");
                }
                catch
                {
                    await messageService.Error(Errors.NewFlowError);
                }
            };
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
