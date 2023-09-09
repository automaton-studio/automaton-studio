using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Automaton.Studio.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapsed;

        [Inject] NavigationManager NavigationManager { get; set; }

        public Guid Id { get; private set; }

        [Parameter] public bool Collapsed 
        { 
            get 
            {  
                return collapsed; 
            } 
            set
            {
                collapsed = value;
                MenuParameters = GetMenuParameters();
            }
        }

        public Type Menu { get; set; } = typeof(MainMenu);
        public Dictionary<string, object> MenuParameters { get; set; }

        protected override async Task OnInitializedAsync()
        {
            MenuParameters = GetMenuParameters();
            NavigationManager.RegisterLocationChangingHandler(NavigationLocationChanged);
            await base.OnInitializedAsync();
        }

        private ValueTask NavigationLocationChanged(LocationChangingContext context)
        {
            if (context.TargetLocation.Contains("flowdesigner"))
            {
                var id = context.TargetLocation.Split('/')[1];
                Guid.TryParse(id, out var flowId);
                NavigateToFlow(flowId);
            }
            else if(context.TargetLocation.Equals(NavigationManager.BaseUri, StringComparison.OrdinalIgnoreCase))
            {
                NavigateToFlows();
            }

            return ValueTask.CompletedTask;
        }

        private Dictionary<string,object> GetMenuParameters()
        {
            return new Dictionary<string, object>()
            {
                { "Collapsed", Collapsed },
                { "Id", Id }
            };
        }

        public void NavigateToFlow(Guid flowId)
        {
            Id = flowId;
            MenuParameters = GetMenuParameters();
            Menu = typeof(FlowMenu);
            InvokeAsync(StateHasChanged);
        }

        public void NavigateToFlows()
        {
            MenuParameters = GetMenuParameters();
            Menu = typeof(MainMenu);
            InvokeAsync(StateHasChanged);
        }
    }
}
