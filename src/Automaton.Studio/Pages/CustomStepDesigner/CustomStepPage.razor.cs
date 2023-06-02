using AntDesign;
using Automaton.Core.Enums;
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
        public IEnumerable<VariableType> VariableTypes { get; } = Enum.GetValues<VariableType>();
        private TypographyEditableConfig stepNameEditableConfig;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private CustomStepViewModel StepDesignerViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        [Parameter] public string StepId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await StepDesignerViewModel.Load(Guid.Parse(StepId));

            stepNameEditableConfig = new() 
            { 
                OnChange = OnNameChanged, 
                Text = StepDesignerViewModel.CustomStep.Name 
            };

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

        private void BackToCustomSteps()
        {
            NavigationManager.NavigateTo("/customsteps");
        }

        private void OnNameChanged(string name)
        {
            StepDesignerViewModel.CustomStep.Name = name;
            StateHasChanged();
        }
    }
}
