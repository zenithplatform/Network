using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Messages
{
    public class QueryNodesRequest : HubRequestMessageBase
    {
        public bool All { get; set; }
        public IEnumerable<NodeIdentifier> Identifiers { get; set; }
    }

    public class QueryNodesResponse : HubResponseMessageBase
    {
        public List<NodeMetadata> NodesMetadata { get; set; }
    }
}
