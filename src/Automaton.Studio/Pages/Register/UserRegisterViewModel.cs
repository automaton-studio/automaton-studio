using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Register;

public class UserRegisterViewModel
{
    private readonly UserRegisterService registrationService;

    public UserRegisterModel UserRegisterDetails { get; set;  } = new UserRegisterModel();

    public UserRegisterViewModel(UserRegisterService registrationService)
    {
        this.registrationService = registrationService;
    }

    public async Task Register()
    {
        await registrationService.Register(UserRegisterDetails);
    }
}
