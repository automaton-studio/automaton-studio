
using AntDesign;
using AutoMapper;
using Automaton.Studio.Components.NewVariable;
using Automaton.Studio.Domain;
using Automaton.Studio.Resources;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Drawer
{
    public partial class FlowVariables
    {
        private Flow flow;
        private FluentValidationValidator fluentValidationValidator;

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private ModalService ModalService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            flow = this.Options;
        }

        public async Task OpenAddVariableDialog()
        {
            var newDefinitionModel = new NewVariableModel
            {
                ExistingNames = flow.VariablesDictionary.Keys
            };

            var newVariableDialog = await ModalService.CreateModalAsync<NewVariableDialog, NewVariableModel>
            (
                new ModalOptions { Title = Labels.Variable }, newDefinitionModel
            );

            newVariableDialog.OnOk = () =>
            {
                flow.AddVariable(newDefinitionModel.Name, newDefinitionModel.Value);

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        public async Task Cancel()
        {
            // Close drawer
            await CloseFeedbackAsync();
        }
    }
}
