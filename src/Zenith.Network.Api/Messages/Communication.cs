using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Messages
{
    public class CommunicationRequest : HubRequestMessageBase
    {
        public string OriginNodeId { get; set; }
        public string TargetNodeId { get; set; }
    }

    public class CommunicationStart : HubResponseMessageBase
    {
        public NodeMetadata Node { get; set; }
    }

    //public class CommunicationRequestCheck : HubResponseMessageBase
    //{
    //    public NodeMetadata OriginNode { get; set; }
    //}

    //public class CommunicationRequestCheckReply : HubRequestMessageBase
    //{
    //    public bool Ready { get; set; }
    //}

    //public class CommunicationAck : HubResponseMessageBase
    //{
    //    public NodeMetadata TargetNode { get; set; }
    //}
}
