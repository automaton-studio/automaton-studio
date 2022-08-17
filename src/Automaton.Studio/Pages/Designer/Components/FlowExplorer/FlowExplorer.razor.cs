using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.Designer.Components.NewDefinition;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components.FlowExplorer;

partial class FlowExplorer : ComponentBase
{
    private string searchText { get; set; }

    [CascadingParameter]
    private StudioFlow Flow { get; set; }

    [Inject]
    private FlowExplorerViewModel FlowExplorerViewModel { get; set; }

    [Inject]
    private ModalService ModalService { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitializedAsync();
    }

    public void UpdateState()
    {
        StateHasChanged();
    }

    private async Task RenameDefinition(FlowExplorerDefinition definition)
    {
        var definitionModel = new NewDefinitionModel()
        {
            Name = definition.Name,
            ExistingNames = FlowExplorerViewModel.DefinitionNames
        };

        var modalRef = await ModalService.CreateModalAsync<NewDefinitionDialog, NewDefinitionModel>
        (
            new ModalOptions { Title = Labels.RenameWorkflow }, 
            definitionModel
        );

        modalRef.OnOk = () =>
        {
            FlowExplorerViewModel.RenameDefinition(definition.Id, definitionModel.Name);
            StateHasChanged();

            return Task.CompletedTask;
        };  
    }

    private void SetStartupDefinition(FlowExplorerDefinition definition)
    {
        FlowExplorerViewModel.SetStartupDefinition(definition.Id);
    }

    private void DeleteDefinition(FlowExplorerDefinition definition)
    {
        FlowExplorerViewModel.DeleteDefinition(definition);
    }

    private string GetClassForDefinition(FlowExplorerDefinition definition)
    {
        return definition.IsStartup ? "selected-definition" : string.Empty;
    }

    private async Task OnSearchTextChange(string value)
    {
        throw new NotImplementedException();
    }

    private async Task OnSearch()
    {
        throw new NotImplementedException();
    }

    private async Task OnEnter(KeyboardEventArgs e)
    {
        throw new NotImplementedException();
    }
}
