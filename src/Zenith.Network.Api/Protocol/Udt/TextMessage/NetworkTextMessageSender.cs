using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api
{
    public class NetworkTextMessageSender : INetworkSendStrategy
    {
        INetworkConnector<Udt.Socket> _connector = null;

        public NetworkTextMessageSender(INetworkConnector<Udt.Socket> connector)
        {
            _connector = connector;
        }

        public void Send<T>(T payload) where T : class
        {
            string message = payload as string;

            _connector.InitializeConnection();

            if (_connector.IsConnected)
            {
                int ini = Environment.TickCount;

                using (Udt.NetworkStream netStream = new Udt.NetworkStream(_connector.GetConnection()))
                using (BinaryReader reader = new BinaryReader(netStream))
                using (BinaryWriter writer = new BinaryWriter(netStream))
                {
                    int messageLength = message.Length;

                    writer.Write(message.Length);
                    byte[] buffer = new byte[messageLength];

                    Array.Copy(Encoding.UTF8.GetBytes(message), buffer, messageLength);

                    _connector.GetConnection().Send(buffer, 0, messageLength);

                    if (!reader.ReadBoolean())
                    {
                        //Console.WriteLine("Error in transmission");
                        return;
                    }
                }
            }
        }
    }
}
