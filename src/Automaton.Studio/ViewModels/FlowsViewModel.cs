using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class FlowsViewModel : IFlowViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IMapper mapper;
        private IFlowsService flowsService;
        private IFlowService flowService;

        #endregion

        #region Properties

        private IEnumerable<FlowModel> flows;
        public IEnumerable<FlowModel> Flows
        {
            get => flows;

            set
            {
                flows = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public FlowsViewModel
        (
            IFlowsService flowsService,
            IFlowService flowService,
            IMapper mapper
        )
        {
            this.flowsService = flowsService;
            this.flowService = flowService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FlowModel>> GetFlows()
        {
            return await flowsService.List();
        }

        /// <summary>
        /// Creates a new flow
        /// </summary>
        public async Task CreateFlow(string name)
        {
            var flow = new Flow() { Name = name };

            await flowService.Save(flow);
        }

        /// <summary>
        /// Deletes a flow
        /// </summary>
        /// <param name="flow">Flow Id to delete</param>
        public void DeleteFlow(string flowId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs flow on its selected runners
        /// </summary>
        /// <param name="flow">Flow model to run</param>
        public async Task RunFlow(FlowModel flow)
        {
            throw new NotImplementedException();
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
