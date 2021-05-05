using Elsa.Models;
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

        public string? Tag { get; set; }
        public bool IsLatest { get; set; }
        public bool IsPublished { get; set; }
        public bool DeleteCompletedInstances { get; set; }
        public bool IsSingleton { get; set; }
        public int Version { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public string? Name { get; set; }
        public string? TenantId { get; set; }
        public string? VersionId { get; }
        public string? DefinitionId { get; set; }
        public WorkflowPersistenceBehavior PersistenceBehavior { get; set; }
        public WorkflowContextOptions? ContextOptions { get; set; }
        public Variables? CustomAttributes { get; set; }
        public Variables? Variables { get; set; }
        public ICollection<ActivityDefinition>? Activities { get; set; }
        public ICollection<ConnectionDefinition>? Connections { get; set; }

        #endregion

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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
