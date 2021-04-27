using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Models
{
    public class WorkflowModel : INotifyPropertyChanged
    {
        #region Elsa

        public bool IsLatest { get; set; }
        public bool IsPublished { get; set; }
        public bool DeleteCompletedInstances { get; set; }
        public bool IsSingleton { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public string? Name { get; set; }
        public string? TenantId { get; set; }
        public string? DefinitionId { get; set; }
        public string? Id { get; set; }
        public int Version { get; set; }

        #endregion

        #region Properties

        private IEnumerable<Guid>? runnerIds;
        public IEnumerable<Guid>? RunnerIds
        {
            get => runnerIds;

            set
            {
                runnerIds = value;
                OnPropertyChanged();
            }
        }

        public bool HasRunners
        {
            get { return RunnerIds != null && RunnerIds.Any(); }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
