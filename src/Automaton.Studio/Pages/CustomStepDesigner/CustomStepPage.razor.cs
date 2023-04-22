using AntDesign;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomStepDesigner
{
    partial class CustomStepPage : ComponentBase
    {
        private bool loading = false;
        private Form<CustomStep> form;
        private IEnumerable<string> VariableTypes { get; } = Enum.GetNames(typeof(VariableType));

        [Inject] private CustomStepViewModel StepDesignerViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        [Parameter] public string StepId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await StepDesignerViewModel.Load(Guid.Parse(StepId));

            await base.OnInitializedAsync();
        }

        public async Task AddInputVariable()
        {
            var inputVariable = new InputVariableModel();

            var modalRef = await ModalService.CreateModalAsync<InputVariableDialog, InputVariableModel>
            (
                new ModalOptions { Title = "New variable" }, inputVariable
            );

            modalRef.OnOk = async () =>
            {
                try
                {
                    StepDesignerViewModel.AddInputVariable(inputVariable);
                    await InvokeAsync(StateHasChanged);
                }
                catch
                {
                    await MessageService.Error($"Input variable {inputVariable.Name} could not be created");
                }

                StateHasChanged();
            };
        }

        public void DeleteInputVariable(string name)
        {
            StepDesignerViewModel.DeleteInputVariable(name);
        }

        public void AddOutputVariable()
        {
            StepDesignerViewModel.AddOutputVariable();
        }

        public void DeleteOutputVariable(string name)
        {
            StepDesignerViewModel.DeleteOutputVariable(name);
        }

        private async Task Save()
        {
            try
            {
                loading = true;

                if (form.Validate())
                {
                    await StepDesignerViewModel.Save();
                }
            }
            catch (Exception ex)
            {
                await MessageService.Error(Resources.Errors.CustomStepUpdateFailed);
            }
            finally
            {
                loading = false;
            }
        }
    }
}
