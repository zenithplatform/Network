using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared.EventAggregation;
using Zenith.Network.Api;

namespace Zenith.Network.Servers.Infrastructure
{
    public class NodeConnectedEvent : IEvent
    {
        public DateTime Timestamp { get; set; }
        public IPEndPoint RemoteEndpoint { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class NodeDisconnectedEvent : IEvent
    {
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
    }

    public class NodeMessageReceivedEvent : IEvent
    {
        public string Id { get; set; }
        public object Message { get; set; }
    }

    public class NodeStatusChanged : IEvent
    {
        public string Id { get; set; }
        public NodeStatus Status { get; set; }
    }

    public class NodeActivityChanged : IEvent
    {
        public string Id { get; set; }
        public NodeActivity CurrentActivity { get; set; }
    }

    public class ServerError : IEvent
    {
        public Exception ExceptionDetails { get; set; }
    }

    public class ServerStarted : IEvent
    {

    }

    public class ServerStopped : IEvent
    {

    }
}
