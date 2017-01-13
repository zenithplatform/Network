using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared;
using Zenith.Network.Api.Base;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api
{
    //This is part of the p2pcopy project, by Pablo hosted on https://github.com/psantosl/p2pcopy 
    public class NetworkFileReceiver : INetworkReceiveStrategy
    {
        INetworkConnector<Udt.Socket> _connector = null;

        public NetworkFileReceiver(INetworkConnector<Udt.Socket> connector)
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
                    string fileName = reader.ReadString();
                    long size = reader.ReadInt64();

                    byte[] buffer = new byte[4 * 1024 * 1024];

                    int i = 0;

                    //ConsoleProgress.Draw(i++, 0, size, ini, Console.WindowWidth / 2);

                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        long read = 0;

                        while (read < size)
                        {
                            int toRecv = reader.ReadInt32();

                            ReadFragment(reader, toRecv, buffer);

                            fileStream.Write(buffer, 0, toRecv);

                            read += toRecv;

                            writer.Write(true);

                            //ConsoleProgress.Draw(i++, read, size, ini, Console.WindowWidth / 2);
                        }
                    }
                }
            }
        }

        int ReadFragment(BinaryReader reader, int size, byte[] buffer)
        {
            int read = 0;

            while (read < size)
            {
                read += reader.Read(buffer, read, size - read);
            }

            return read;
        }
    }
}
