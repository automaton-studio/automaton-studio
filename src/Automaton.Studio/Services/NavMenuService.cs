namespace Automaton.Studio.Services
{
    public class NavMenuService
    {
        private bool designerDisabled = true;

        public void EnableDesignerMenu()
        {
            designerDisabled = false;
        }

        public void DisableDesignerMenu()
        {
            designerDisabled = true;
        }

        public bool IsDesignerDisabled()
        {
            return designerDisabled;
        }
    }
}
