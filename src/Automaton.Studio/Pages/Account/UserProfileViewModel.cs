using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

public class UserProfileViewModel
{
    private readonly UserAccountService userAccountService;

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public UserProfileViewModel(UserAccountService userAccountService)
    {
        this.userAccountService = userAccountService;
    }

    public async Task LoadUserProfile()
    {
        var userProfile = await userAccountService.GetUserProfile();

        UserName = userProfile.UserName; 
        FirstName = userProfile.FirstName; 
        LastName = userProfile.LastName;
        Email = userProfile.Email;
    }

    public async Task UpdateUserProfile()
    {
        var userProfile = new Models.UserProfile
        {
            UserName = UserName,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email
        };

        await userAccountService.UpdateUserProfile(userProfile);
    }
}
