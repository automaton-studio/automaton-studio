using MediatR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Automaton.Core.Events
{
    public class HubConnectionNotification : INotification
    {
        public HubConnectionState HubConnectionState { get; private set; }

        public HubConnectionNotification(HubConnectionState hubConnectionState)
        {
            HubConnectionState = hubConnectionState;
        }
    }
}
