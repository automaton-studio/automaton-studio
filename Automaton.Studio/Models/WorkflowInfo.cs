using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Models
{
    /// <summary>
    /// Workflow information
    /// </summary>
    public class WorkflowInfo : INotifyPropertyChanged
    {
        #region Elsa properties

        public string Tag { get; set; }
        public bool IsLatest { get; set; }
        public bool IsPublished { get; set; }
        public bool DeleteCompletedInstances { get; set; }
        public bool IsSingleton { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; }
        public string VersionId { get; }
        public string DefinitionId { get; set; }

        #endregion

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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
