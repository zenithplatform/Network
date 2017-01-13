using System;

namespace Zenith.Network.Api.Messages
{
    //Base request message
    public class HubRequestMessageBase
    {
        public string ConnectionId { get; set; }
    }

    //Base response message
    public class HubResponseMessageBase
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }
    }
}
