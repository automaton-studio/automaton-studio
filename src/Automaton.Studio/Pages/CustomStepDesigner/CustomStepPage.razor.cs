using AntDesign;
using Automaton.Core.Enums;
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

        [Inject] private CustomStepViewModel StepDesignerViewModel { get; set; } = default!;
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

        public void AddInputVariable()
        {
            StepDesignerViewModel.AddInputVariable();
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

        private async Task Test()
        {
            try
            {
                StepDesignerViewModel.Execute();
            }
            catch (Exception ex)
            {
                await MessageService.Error(Resources.Errors.CustomStepUpdateFailed);
            }
        }

        private void OnNameChanged(string name)
        {
            StepDesignerViewModel.CustomStep.Name = name;
            StateHasChanged();
        }
    }
}
