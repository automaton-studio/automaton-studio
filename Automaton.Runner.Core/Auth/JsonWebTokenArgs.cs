using System;

namespace Automaton.Runner.Core
{
    public class JsonWebTokenArgs : EventArgs
    {
        public JsonWebToken AuthToken { get; set; }
    }
}
