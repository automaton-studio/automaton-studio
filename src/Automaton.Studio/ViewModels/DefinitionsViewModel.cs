using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class DefinitionsViewModel : IDefinitionsViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IMapper mapper;
        private IDefinitionService definitionService;

        #endregion

        #region Properties

        private IEnumerable<DefinitionModel> definitions;
        public IEnumerable<DefinitionModel> Definitions
        {
            get => definitions;

            set
            {
                definitions = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public DefinitionsViewModel
        (
            IDefinitionService definitionService,
            IMapper mapper
        )
        {
            this.definitionService = definitionService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DefinitionModel>> GetDefinitions()
        {
            var definitions = await this.definitionService.List();
            Definitions = mapper.Map<IEnumerable<Definition>, IEnumerable<DefinitionModel>>(definitions);

            return Definitions;
        }

        /// <summary>
        /// Creates a new flow
        /// </summary>
        public async Task<DefinitionModel> CreateFlow(string flowName)
        {
            throw new NotImplementedException();
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
        public async Task RunFlow(DefinitionModel flow)
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
