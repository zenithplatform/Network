using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Messages
{
    public class NodeRegistrationRequest : HubRequestMessageBase
    {
        public NodeMetadata NodeMetadata { get; set; }
        public bool Unregister { get; set; }

        public NodeRegistrationRequest()
        {

        }
    }

    public class NodeRegistrationResponse : HubResponseMessageBase
    {

    }
}
