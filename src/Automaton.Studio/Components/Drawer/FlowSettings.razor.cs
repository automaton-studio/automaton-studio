
using AntDesign;
using AutoMapper;
using Automaton.Studio.Domain;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Drawer
{
    public partial class FlowSettings
    {
        private Flow flow;
        private FluentValidationValidator fluentValidationValidator;

        [Inject]
        private IMapper Mapper { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            flow = this.Options;
        }

        public async Task Submit()
        {
            if (fluentValidationValidator.Validate(options => options.IncludeAllRuleSets()))
            {
                // Close drawer and return true
                var drawerRef = base.FeedbackRef as DrawerRef<bool>;
                await drawerRef!.CloseAsync(true);
            }
        }

        public async Task Cancel()
        {
            // Close drawer
            await CloseFeedbackAsync();
        }
    }
}
