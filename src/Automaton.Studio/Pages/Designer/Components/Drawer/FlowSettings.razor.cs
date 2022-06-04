
using AntDesign;
using Automaton.Studio.Domain;
using Blazored.FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components.Drawer
{
    public partial class FlowSettings
    {
        private StudioFlow flow;
        private FluentValidationValidator fluentValidationValidator;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            flow = this.Options;
        }

        public async Task Submit()
        {
            if (fluentValidationValidator.Validate(options => options.IncludeAllRuleSets()))
            {
                var drawerRef = base.FeedbackRef as DrawerRef<bool>;
                await drawerRef!.CloseAsync(true);
            }
        }

        public async Task Cancel()
        {
            await CloseFeedbackAsync();
        }
    }
}
