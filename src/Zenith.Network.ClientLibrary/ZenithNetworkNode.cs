using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api.Infrastructure;

namespace Zenith.Network.Client
{
    public class ZenithNetworkNode : NetworkNode
    {
        public override void Initialize()
        {
            Register("Global", () => new GlobalNetworkCoordinator());
        }
    }
}
