using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IWorkflowsViewModel
    {
        IEnumerable<WorkflowModel> Workflows { get; set; }
        IEnumerable<RunnerModel> Runners { get; set; }

        Task Initialize();
        Task RunWorkflow(WorkflowModel workflow);     
    }
}
