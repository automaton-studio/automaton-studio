using Automaton.Studio.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface ILoginViewModel
    {
        LoginModel Model { get; set; }
        Task Login();
    }
}
    