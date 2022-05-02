namespace Automaton.Runner.ViewModels.Common
{
    public interface IViewModelLoader
    {
        void SetErrors(string errors);
        void ClearErrors();
        void StartLoading();
        void StopLoading();
    }
}
