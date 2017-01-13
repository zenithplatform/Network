using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class LocalNetworkHelper
    {
        UdpClient _receiver, _sender = null;
        IPEndPoint _remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
        IPEndPoint _receiverEndpoint = new IPEndPoint(IPAddress.Any, BroadcastConfig.AutoDiscoveryPort);

        IPAddress _localAddress = IPAddress.Any;
        int _localPort = 0;

        bool _acknowledge = false;
        volatile bool _receiving = false;
        volatile bool _broadcasting = false;

        int _broadcastInterval = 1000;

        Thread _receiveThread, _broadcastThread = null;

        public event EventHandler OnDiscovered;

        public LocalNetworkHelper(IPAddress localAddress)
        {
            _localAddress = localAddress;
        }

        public void Announce()
        {
            _broadcasting = true;
            InitializeBroadcaster();
        }

        public void StartDiscovering()
        {
            _receiving = true;
            InitializeReceiver();
        }

        public void TurnOff()
        {
            StopReceiving();
            StopBroadcasting();
        }

        private void InitializeReceiver()
        {
            _receiver = new UdpClient(_receiverEndpoint);

            _receiveThread = new Thread(new ThreadStart(StartReceiving));
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        private void StartReceiving()
        {
            byte[] receiveBuffer = new byte[1024];

            try
            {
                receiveBuffer = _receiver.Receive(ref _remoteEndpoint);

                while (_receiving)
                {
                    if (receiveBuffer.SequenceEqual(BroadcastConfig.PacketBytes))
                    {
                        if (_acknowledge)
                        {
                            byte[] ackPacket = Encoding.ASCII.GetBytes(string.Format("ACK:{0:1}", _localAddress, BroadcastConfig.AutoDiscoveryPort));
                            _receiver.Send(ackPacket, ackPacket.Length, _remoteEndpoint);

                            if (OnDiscovered != null)
                                OnDiscovered(this, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        if (_acknowledge)
                        {
                            byte[] nackPacket = Encoding.ASCII.GetBytes("NACK");
                            _receiver.Send(nackPacket, nackPacket.Length, _remoteEndpoint);
                        }
                    }

                    receiveBuffer = _receiver.Receive(ref _remoteEndpoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StopReceiving()
        {   
            _receiving = false;
            _receiver.Close();

            if (!_receiveThread.Join(1000))
                _receiveThread.Abort();
        }

        private void InitializeBroadcaster()
        {
            _sender = new UdpClient();
            _sender.EnableBroadcast = true;
            _sender.MulticastLoopback = false;
            //_sender.ExclusiveAddressUse = false;

            _broadcastThread = new Thread(new ThreadStart(StartBroadcasting));
            _broadcastThread.IsBackground = true;
            _broadcastThread.Start();
        }

        private void StartBroadcasting()
        {
            byte[] receiveBuffer = new byte[1024];

            try
            {
                while (_broadcasting)
                {   
                    _sender.Send(BroadcastConfig.PacketBytes, BroadcastConfig.PacketBytes.Length, new IPEndPoint(IPAddress.Broadcast, BroadcastConfig.AutoDiscoveryPort));
                    Thread.Sleep(_broadcastInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StopBroadcasting()
        {
            _broadcasting = false;
            _sender.Close();

            if (!_broadcastThread.Join(1000))
                _broadcastThread.Abort();
        }

        public bool Acknowledge
        {
            get { return _acknowledge; }
            set { _acknowledge = value; }
        }

        public int AnnounceInterval
        {
            get { return _broadcastInterval; }
            set { _broadcastInterval = value; }
        }
    }
}
