using AntDesign;
using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class ActivitiesTree : ComponentBase
    {
        [Inject] private ITreeActivityViewModel ActivitiesViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await ActivitiesViewModel.Initialize();

        }

        public void DoubleClickActivity(TreeEventArgs<ActivityTreeModel> args, string eventName)
        {
            // Ignore double ckick on groups
            if (args.Node.ChildNodes.Any())
            {
                return;
            }

            var x = $"{eventName}:{args.Node.Title}";
        }
    }
}
