using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class NetworkNodeRegistrationRequest : NetworkNodeRequestBase
    {
        public NodeMetadata NodeMetadata { get; set; }
    }

    public class NetworkNodeUnregistrationRequest : NetworkNodeRequestBase
    {
        public string Name { get; set; }
        public Guid Identifier { get; set; }
    }

    public class NetworkNodeRegistrationResponse : NetworkNodeResponseBase
    {

    }
}
