using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Zenith.Core.Shared;
using Zenith.Network.Api.Base;

namespace Zenith.Network.Api
{
    public class UdtConnector : NetworkConnectorBase<Udt.Socket>
    {
        Udt.Socket _connection = null;
        IPEndPoint _localEndpoint, _remoteEndpoint = null;
        bool bConnected = false;

        public UdtConnector(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
            :base(localEndpoint, remoteEndpoint)
        {
        }

        public override bool IsConnected
        {
            get
            {
                return bConnected;
            }
        }

        public override void InitializeConnection()
        {
            try
            {
                while (!bConnected)
                {
                    try
                    {
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        socket.Bind(new IPEndPoint(_localEndpoint.Address, _localEndpoint.Port));

                        DateTime now = InternetTime.Get();

                        int sleepTimeToSync = Utils.SleepTime(now);
                        System.Threading.Thread.Sleep(sleepTimeToSync * 1000);

                        if (_connection != null)
                            _connection.Close();

                        _connection = new Udt.Socket(AddressFamily.InterNetwork, SocketType.Stream);
                        _connection.SetSocketOption(Udt.SocketOptionName.Rendezvous, true);
                        _connection.Bind(socket);
                        _connection.Connect(_remoteEndpoint.Address, _remoteEndpoint.Port);

                        bConnected = true;
                    }
                    catch (Exception e)
                    {
                        bConnected = false;
                    }
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public override Udt.Socket GetConnection()
        {
            return _connection;
        }
    }
}
