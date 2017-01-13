using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api
{
    public class NetworkTextMessageReceiver : INetworkReceiveStrategy
    {
        INetworkConnector<Udt.Socket> _connector = null;

        public NetworkTextMessageReceiver(INetworkConnector<Udt.Socket> connector)
        {
            _connector = connector;
        }

        public void Receive()
        {
            _connector.InitializeConnection();

            if (_connector.IsConnected)
            {
                int ini = Environment.TickCount;

                using (Udt.NetworkStream netStream = new Udt.NetworkStream(_connector.GetConnection()))
                using (BinaryWriter writer = new BinaryWriter(netStream))
                using (BinaryReader reader = new BinaryReader(netStream))
                {
                    int size = reader.Read();

                    byte[] buffer = new byte[size];
                    reader.Read(buffer, 0, size);
                    string message = Encoding.UTF8.GetString(buffer);

                    writer.Write(true);
                }
            }
        }
    }
}
