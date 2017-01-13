using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Core.Stun;

namespace Zenith.Network.Tests
{
    [TestClass]
    public class Stun
    {
        [TestMethod]
        public void Run()
        {
            // https://gist.github.com/zziuni/3741933
            Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            StunResult externalEndPoint = StunClient.Query("stun.l.google.com", 19302, socket);

            //if (externalEndPoint.NetType == StunNetType.UdpBlocked)
            //{
            //    Console.WriteLine("Your external IP can't be obtained. You are blocked :-(");
            //    return null;
            //}

            //Console.WriteLine("Your firewall is {0}", externalEndPoint.NetType.ToString());

            //return new P2pEndPoint()
            //{
            //    External = externalEndPoint.PublicEndPoint,
            //    Internal = (socket.LocalEndPoint as IPEndPoint)
            //};
        }
    }
}
