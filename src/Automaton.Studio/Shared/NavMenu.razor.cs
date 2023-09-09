using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapsed;

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

        public NavMenu()
        {
            MenuParameters = GetMenuParameters();
        }

        private Dictionary<string,object> GetMenuParameters()
        {
            return new Dictionary<string, object>()
            {
                { "Collapsed", Collapsed }
            };
        }
    }
}
