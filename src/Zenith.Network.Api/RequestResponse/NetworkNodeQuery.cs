using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class NetworkNodeQueryRequest : NetworkNodeRequestBase
    {
        public string Name { get; set; }
        public Guid Identifier { get; set; }
    }

    public class NetworkNodeQueryResponse : NetworkNodeResponseBase
    {
        public NodeMetadata NodeMetadata { get; set; }
    }

    public class NetworkNodeQueryAllRequest : NetworkNodeRequestBase
    {

    }

    public class NetworkNodeQueryAllResponse : NetworkNodeResponseBase
    {
        public List<NodeMetadata> NodeMetadata { get; set; }
    }
}
