using Microsoft.AspNetCore.Components;

namespace Automaton.App.Account.Account;

partial class AccountPage : ComponentBase
{
    private Type accountSection = typeof(UserProfile);

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void NavigateToUserProfile()
    {
        accountSection = typeof(UserProfile);
    }

    private void NavigateToUserSecurity()
    {
        accountSection = typeof(UserSecurity);
    }
}
