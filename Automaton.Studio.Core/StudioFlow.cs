using System;
using System.Collections.Generic;

namespace Automaton.Studio.Core
{
    public class StudioFlow
    {
        #region Public Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupWorkflowId { get; set; }

        /// <summary>
        /// List of workflows
        /// </summary>
        public IList<StudioWorkflow> Workflows { get; set; }

        public StudioWorkflow ActiveWorkflow { get; set; }

        #endregion

        #region Constructors

        public StudioFlow()
        {
            Name = "Untitled";
            ActiveWorkflow = new StudioWorkflow();
            Workflows = new List<StudioWorkflow> { ActiveWorkflow };
        }

        #endregion
    }
}
