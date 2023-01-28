using AntDesign;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account
{
    public partial class UserSecurity : ComponentBase
    {
        private bool loading = false;

        [Inject] private UserSecurityViewModel UserSecurityViewModel { get; set; } = default!;
        [Inject] public NavMenuService NavMenuService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFinish(EditContext editContext)
        {
            try
            {
                loading = true;

                await UserSecurityViewModel.UpdateUserPassword();

                await MessageService.Info(Resources.Information.UserPasswordUpdated);
            }
            catch (Exception ex)
            {
                await MessageService.Error(Resources.Errors.UserProfileUpdateFailed);
            }
            finally
            {
                loading = false;
            }
        }

        private void OnFinishFailed(EditContext editContext)
        {
            // Do nothing if form validation fails
        }
    }
}
