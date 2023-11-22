using AntDesign;
using Automaton.Core.Enums;
using Automaton.Studio.Domain;
using Automaton.Studio.Steps.Custom.Variables;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.CustomStepDesigner
{
    partial class CustomStepPage : ComponentBase
    {
        private bool loading = false;
        private bool runningCode = false;

        private Form<CustomStep> form;
        private Form<CustomStep> codeForm;

        public IEnumerable<VariableType> VariableTypes { get; } = Enum.GetValues<VariableType>();
        private TypographyEditableConfig stepNameEditableConfig;

        [Inject] private CustomStepViewModel StepDesignerViewModel { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        [Parameter] public string StepId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StepDesignerViewModel.ScriptTextWritten += OnScriptOutputWritten;

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

        private async Task Run()
        {
            try
            {
                runningCode = true;

                await Task.Delay(100);

                StepDesignerViewModel.Execute();

                await MessageService.Success(Resources.Information.CustomStepExecutionSuccessful);
            }
            catch (Exception ex)
            {
                await MessageService.Error($"{Resources.Errors.CustomStepExecutionFailed}. Error: {ex.Message}");
            }
            finally
            {
                runningCode = false;
            }
        }

        private Type GetVariableComponent(VariableType type)
        {
            return type switch
            {
                VariableType.String => typeof(StringVariable),
                VariableType.Text => typeof(TextVariable),
                VariableType.Boolean => typeof(BooleanVariable),
                VariableType.Number => typeof(NumericVariable),
                VariableType.Date => typeof(DateVariable),
                _ => throw new NotImplementedException()
            };
        }

        private Dictionary<string, object> GetVariableComponentParameters(Core.Models.StepVariable customStepVariable)
        {
            return new Dictionary<string, object>() { { "Variable", customStepVariable } };
        }

        private void OnNameChanged(string name)
        {
            StepDesignerViewModel.CustomStep.Name = name;
            InvokeAsync(StateHasChanged);
        }

        private void OnScriptOutputWritten(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            StepDesignerViewModel.ScriptTextWritten -= OnScriptOutputWritten;
        }
    }
}
