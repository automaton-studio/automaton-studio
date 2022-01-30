using Automaton.Studio.Services.Interfaces;

namespace Automaton.Studio.Services
{
    public class NavMenuService : INavMenuService
    {
        private bool designerDisabled = true;

        public void DisableDesignerMenu()
        {
            designerDisabled = true;
        }

        public void EnableDesignerMenu()
        {
            designerDisabled = false;
        }

        public bool IsDesignerDisabled()
        {
            return designerDisabled;
        }
    }
}
