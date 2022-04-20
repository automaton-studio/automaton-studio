using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    public interface ILoginViewModel
    {
        LoginModel LoginModel { get; set; }
        Task<bool> Login();
    }
}
    