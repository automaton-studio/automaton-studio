using Common.Authentication;
using MediatR;

namespace Automaton.Studio.Server.Core.Commands
{
    public class RefreshAccessTokenCommand
    {
        public string Token { get; }

        public RefreshAccessTokenCommand(string token)
        {
            Token = token;
        }
    }
}