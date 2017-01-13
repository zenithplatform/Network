using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class NodeConnectedEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; }
        public IPEndPoint RemoteEndpoint { get; set; }
        public string Id { get; set; }
    }

    public class NodeDisconnectedEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
    }

    public class NodeMessageReceivedEventArgs : EventArgs
    {
        public string Id { get; set; }
        public object Message { get; set; }
    }

    public class NodeStatusChangedArgs : EventArgs
    {
        public string Id { get; set; }
        public NodeStatus Status { get; set; }
    }

    public class NodeActivityChangeArgs : EventArgs
    {
        public string Id { get; set; }
        public NodeActivity CurrentActivity { get; set; }
    }
}
