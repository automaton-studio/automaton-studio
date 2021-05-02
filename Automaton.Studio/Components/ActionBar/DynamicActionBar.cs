using Automaton.Studio.Components.ActionBar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;

namespace Automaton.Studio.Components
{
    /// <summary>
    /// A code-based component that renders action bar using its GetViewComponent result
    /// </summary>
    public class DynamicActionBar : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// The ActionBar we want to render
        /// </summary>
        [Parameter]
        public ActionBar.ActionBar? ActionBar { get; set; }

        /// <summary>
        /// Render the component
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ActionBar == null)
            {
                throw new ArgumentException("Invalid ActionBar parameter");
            }

            var routeParts = NavigationManager.Uri.Split("/");
            var routeName = routeParts.LastOrDefault();
            var actionBar = ActionBarFactory.GetActionBar(routeName);

            base.BuildRenderTree(builder);

            // get the component to view the product with
            Type componentType = actionBar.GetViewComponent();
            // create an instance of this component
            builder.OpenComponent(0, componentType);
            // close
            builder.CloseComponent();
        }
    }
}
