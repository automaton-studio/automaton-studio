using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Runners;

partial class RunnersPage : ComponentBase
{
    public bool loading;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private RunnersViewModel RunnersViewModel { get; set; } = default!;
    [Inject] private ModalService ModalService { get; set; }
    [Inject] private MessageService MessageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        try
        {
            await RunnersViewModel.GetRunners();
        }
        catch
        {
            await MessageService.Error(Resources.Errors.FlowsListNotLoaded);
        }
        finally
        {
            loading = false;
        }

        await base.OnInitializedAsync();
    }
}
