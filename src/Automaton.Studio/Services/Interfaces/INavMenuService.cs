namespace Automaton.Studio.Services.Interfaces
{
    public interface INavMenuService
    {
        bool IsDesignerDisabled();

        void DisableDesignerMenu();

        void EnableDesignerMenu();
    }
}
