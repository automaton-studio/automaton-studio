using AntDesign;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

partial class AccountPage : ComponentBase
{
    [Inject] private AccountViewModel AccountViewModel { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public NavMenuService NavMenuService { get; set; }
    [Inject] private MessageService MessageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task Logout()
    {
        await AccountViewModel.Logout();

        NavMenuService.DisableDesignerMenu();

        NavigationManager.NavigateTo($"/");
    } 
}
