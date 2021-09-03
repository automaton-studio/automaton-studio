using AutoMapper;
using Automaton.Studio.Core.Metadata;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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

        public void LoadFlow(string flowId)
        {
            Workflows = new List<WorkflowModel>();

            // TODO: Init list of workflows
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
