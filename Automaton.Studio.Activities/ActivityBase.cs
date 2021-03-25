using Elsa.Services;
using System;

namespace Automaton.Studio.Activities
{
    /// <summary>
    /// A base class for all activity instances
    /// </summary>
    public abstract class ActivityBase
    {
        #region Properties

        public string Id
        {
            get { return ElsaActivity.Id; }
            set { ElsaActivity.Id = value; }
        }

        public string? Name
        {
            get { return ElsaActivity.Name; }
            set { ElsaActivity.Name = value; }
        }

        public string? Description
        {
            get { return ElsaActivity.Description; }
            set { ElsaActivity.Description = value; }
        }

        public string? DisplayName
        {
            get { return ElsaActivity.DisplayName; }
            set { ElsaActivity.DisplayName = value; }
        }

        public string Type
        {
            get { return ElsaActivity.Type; }
        }
        #endregion

        #region Abstracts

        /// <summary>
        /// Corresponding Elsa activity
        /// </summary>
        public abstract IActivity ElsaActivity { get; }

        /// <summary>
        /// Abstract method to get the view component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetViewComponent();

        /// <summary>
        /// Abstract method to get the properties component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetPropertiesComponent();

        #endregion
    }
}
