using System;

namespace Automaton.Runner.Core
{
    public class AuthTokenArgs : EventArgs
    {
        public JsonWebToken AuthToken { get; set; }
    }
}
