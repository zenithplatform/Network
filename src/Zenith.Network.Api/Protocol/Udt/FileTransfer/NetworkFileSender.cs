using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared;
using Zenith.Core.Shared.Helpers;
using Zenith.Network.Api.Base;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api
{
    //This is part of the p2pcopy project, by Pablo hosted on https://github.com/psantosl/p2pcopy 
    public class NetworkFileSender : INetworkSendStrategy
    {
        INetworkConnector<Udt.Socket> _connector = null;

        public NetworkFileSender(INetworkConnector<Udt.Socket> connector)
        {
            _connector = connector;
        }

        public void Send<T>(T payload) where T : class
        {
            string fileName = payload as string;

            _connector.InitializeConnection();

            if (_connector.IsConnected)
            {
                int ini = Environment.TickCount;

                using (Udt.NetworkStream netStream = new Udt.NetworkStream(_connector.GetConnection()))
                using (BinaryWriter writer = new BinaryWriter(netStream))
                using (BinaryReader reader = new BinaryReader(netStream))
                using (FileStream fileReader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    long fileSize = new FileInfo(fileName).Length;

                    writer.Write(Path.GetFileName(fileName));
                    writer.Write(fileSize);

                    byte[] buffer = new byte[512 * 1024];

                    long pos = 0;

                    int i = 0;

                    //ConsoleProgress.Draw(i++, pos, fileSize, ini, Console.WindowWidth / 3);

                    while (pos < fileSize)
                    {
                        int toSend = buffer.Length < (fileSize - pos)
                            ? buffer.Length
                            : (int)(fileSize - pos);

                        fileReader.Read(buffer, 0, toSend);

                        int iteration = Environment.TickCount;

                        writer.Write(toSend);
                        _connector.GetConnection().Send(buffer, 0, toSend);

                        if (!reader.ReadBoolean())
                        {
                            //Console.WriteLine("Error in transmission");
                            return;
                        }

                        pos += toSend;

                        //ConsoleProgress.Draw(i++, pos, fileSize, ini, Console.WindowWidth / 3);

                        //if (bVerbose)
                        //{
                        //    //Console.WriteLine();

                        //    //Console.WriteLine("Current: {0} / s",
                        //    //    SizeConverter.ConvertToSizeString(toSend / (Environment.TickCount - iteration) * 1000));

                        //    //Console.WriteLine("BandwidthMbps {0} mbps.", conn.GetPerformanceInfo().Probe.BandwidthMbps);
                        //    //Console.WriteLine("RoundtripTime {0}.", conn.GetPerformanceInfo().Probe.RoundtripTime);
                        //    //Console.WriteLine("SendMbps {0}.", conn.GetPerformanceInfo().Local.SendMbps);
                        //    //Console.WriteLine("ReceiveMbps {0}.", conn.GetPerformanceInfo().Local.ReceiveMbps);
                        //}
                    }
                }
            }
        }
    }
}
