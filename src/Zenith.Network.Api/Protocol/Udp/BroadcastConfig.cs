using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class BroadcastConfig
    {
        public static byte[] PacketBytes = new byte[] { 0x1, 0x2, 0x3 };
        public static int AutoDiscoveryPort = 18500;
    }
}
