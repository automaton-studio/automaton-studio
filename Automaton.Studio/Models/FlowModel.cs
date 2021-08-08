using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Models
{
    public class FlowModel : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string StartupWorkflowId { get; set; }

        private IEnumerable<Guid> runnerIds = new List<Guid>();
        public IEnumerable<Guid> RunnerIds
        {
            get => runnerIds;

            set
            {
                runnerIds = value;
                OnPropertyChanged();
            }
        }

        #region Public Methods

        public void Reset()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            UserId = string.Empty;
            StartupWorkflowId = string.Empty;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
