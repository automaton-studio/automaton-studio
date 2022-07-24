
using AntDesign;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.Designer.Components.NewVariable;
using Automaton.Studio.Resources;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components.Drawer
{
    public partial class FlowVariables
    {
        private StudioFlow flow;
        private FluentValidationValidator fluentValidationValidator;

        [Inject] private ModalService ModalService { get; set; } = default!;

        private IEnumerable<Variable> Variables
        {
            get
            {
                return flow.Variables.Select(x => new Variable
                {
                    Name = x.Key,
                }).OrderBy(x => x.Name);
            }
        }

        private IEnumerable<Variable> OutputVariables
        {
            get
            {
                return flow.OutputVariables.Select(x => new Variable
                {
                    Name = x.Key,
                    Value = x.Value.ToString()
                }).OrderBy(x => x.Name);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            flow = this.Options;
        }

        public async Task AddOutputVariable()
        {
            var newDefinitionModel = new VariableModel
            {
                ExistingNames = flow.GetVariableNames()
            };

            var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
            (
                new ModalOptions { Title = Labels.Variable }, newDefinitionModel
            );

            newVariableDialog.OnOk = () =>
            {
                flow.SetOutputVariable(newDefinitionModel.Name, newDefinitionModel.Value);

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        public async Task EditOutputVariable(Variable variable)
        {
            var outputVariableNames = flow.GetOutputVariableNames();
            var existingOutputVariables = outputVariableNames.Where(x => !x.Equals(variable.Name, StringComparison.OrdinalIgnoreCase));

            var updatedVariable = new VariableModel
            {
                ExistingNames = existingOutputVariables,
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
                    flow.DeleteOutputVariable(variable.Name);
                }

                flow.SetOutputVariable(updatedVariable.Name, updatedVariable.Value);

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
