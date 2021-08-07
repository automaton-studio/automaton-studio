using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Studio.Core
{
    public class StudioFlow
    {
        #region Public Properties

        /// <summary>
        /// List of workflows
        /// </summary>
        public IList<StudioWorkflow> Workflows { get; set; }

        #endregion

        #region Constructors

        public StudioFlow()
        {
            var defaultWorkflow = new StudioWorkflow();
            Workflows = new List<StudioWorkflow> { defaultWorkflow };
        }

        #endregion



    }
}
