using Griffin.Net;
using Griffin.Net.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api;

namespace Zenith.Network.Api
{
    public class NodeTcpClient : ChannelTcpClient
    {
        public NodeTcpClient() 
            : base(new MessageEncoder(), new MessageDecoder(), new BufferSlice(new byte[65535], 0, 65535))
        {

        }
    }
}
