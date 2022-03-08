
using AntDesign;
using AutoMapper;
using Automaton.Studio.Components.NewVariable;
using Automaton.Studio.Domain;
using Automaton.Studio.Resources;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Drawer
{
    public partial class FlowVariables
    {
        private Flow flow;
        private FluentValidationValidator fluentValidationValidator;

        [Inject]
        private ModalService ModalService { get; set; } = default!;

        private IEnumerable<Variable> Variables
        {
            get
            {
                return flow.GetVariables().OrderBy(x => x.Name);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            flow = this.Options;
        }

        public async Task AddVariable()
        {
            var newDefinitionModel = new VariableModel
            {
                ExistingNames = flow.VariablesDictionary.Keys
            };

            var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
            (
                new ModalOptions { Title = Labels.Variable }, newDefinitionModel
            );

            newVariableDialog.OnOk = () =>
            {
                flow.SetVariable(newDefinitionModel.Name, newDefinitionModel.Value);

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        public async Task EditVariable(Variable variable)
        {
            var existingVariables = flow.VariablesDictionary.Keys.Where(x => !x.Equals(variable.Name, StringComparison.OrdinalIgnoreCase));

            var updatedVariable = new VariableModel
            {
                ExistingNames = existingVariables,
                Name = variable.Name,
                Value = variable.Value
            };

            var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
            (
                new ModalOptions { Title = Labels.Variable }, updatedVariable
            );

            newVariableDialog.OnOk = () =>
            {
                if (!variable.Name.Equals(updatedVariable.Name, StringComparison.OrdinalIgnoreCase))
                {
                    flow.DeleteVariable(variable.Name);
                }

                flow.SetVariable(updatedVariable.Name, updatedVariable.Value);

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        public async Task Cancel()
        {
            await CloseFeedbackAsync();
        }
    }
}
