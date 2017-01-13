using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Core.Nat
{
    public class NatMappingEntry
    {
        public int PrivatePort { get; set; }
        public int PublicPort { get; set; }
        public string Description { get; set; }
        public NetworkProtocol Protocol { get; set; }
    }

    public enum NetworkProtocol
    {
        Tcp = 0,
        Udp = 1
    }
}
