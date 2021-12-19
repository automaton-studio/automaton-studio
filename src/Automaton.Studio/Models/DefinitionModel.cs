using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Models
{
    public class DefinitionModel : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
