using AntDesign;
using Blazored.FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps.Components.NewCustomStep;

public partial class NewCustomStepDialog : FeedbackComponent<NewCustomStepModel>
{
    private Form<NewCustomStepModel> form;
    private NewCustomStepModel flowModel;
    private FluentValidationValidator fluentValidationValidator;

    protected override void OnInitialized()
    {
        flowModel = this.Options;
        base.OnInitialized();
    }

    public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
    {
        // Below workaround is required to avoid NewWorkflow dialog
        // close unexpectedly when shown for the first time.
        // The fact that validation happens synchronous (while this method is async) may be the cause of the problem.
        // Please keep both of the next two lines, or find a fix for the issue.
        // Razvan, May 2021
        await InvokeAsync(StateHasChanged);
        await Task.Delay(50);

        args.Cancel = !fluentValidationValidator.Validate(options => options.IncludeAllRuleSets());
    }
}
