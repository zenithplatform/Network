using Griffin.Net;
using Griffin.Net.Channels;
using Griffin.Net.Protocols;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Infrastructure
{
    public class LocalNetworkCoordinator : INetworkCoordinator
    {
        public event EventHandler<NodeMetadata> NodeDiscovered;
        public event EventHandler<object> OnDataReceived;
        public event EventHandler<object> OnDataSent;
        public event EventHandler OnIncomingConnectionEstablished;
        public event EventHandler OnIncomingConnectionTerminated;

        LocalNetworkHelper _localNetworkHelper = null;

        private int _incomingConnectionsPort = 0;
        NodeTcpListener _listener = null;
        NodeTcpClient _client = null;
        ChannelTcpListenerConfiguration _config = null;
        IComponentContainer _container = null;

        public LocalNetworkCoordinator()
        {
            _localNetworkHelper = new LocalNetworkHelper(IPAddress.Any);
            _localNetworkHelper.Acknowledge = false;
            _localNetworkHelper.AnnounceInterval = 2000;
        }

        public void Initialize(IComponentContainer container)
        {
            _container = container;
        }

        public async Task BeginDiscovery()
        {
            await Task.Run(() => {
                _localNetworkHelper.StartDiscovering();
                _localNetworkHelper.OnDiscovered += LocalNetworkHelper_OnDiscovered;
            });
        }

        public void EndDiscovery()
        {
            _localNetworkHelper.OnDiscovered -= LocalNetworkHelper_OnDiscovered;
        }

        private void LocalNetworkHelper_OnDiscovered(object sender, EventArgs e)
        {
            if (NodeDiscovered != null)
                NodeDiscovered(this, null);
        }

        public async Task<bool> Register(NodeMetadata data)
        {
            await Task.Run(() => {
                _localNetworkHelper.Announce();
            });

            return true;
        }

        public async Task<bool> Unregister()
        {
            await Task.Run(() => {
                _localNetworkHelper.TurnOff();
            });

            return true;
        }

        public async Task Send<T>(T data) where T : class
        {
            try
            {
                await _client.SendAsync(data);
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        private bool StartListening()
        {
            bool success = true;

            try
            {
                _config = new ChannelTcpListenerConfiguration(
                                () => new MessageDecoder(),
                                () => new MessageEncoder()
                            );

                _listener = new NodeTcpListener(_config);
                _listener.MessageReceived += OnMessage;
                _listener.ClientConnected += OnClientConnected;
                _listener.ClientDisconnected += OnClientDisconnected;

                _listener.Start(IPAddress.IPv6Any, this.IncomingConnectionsPort);
            }
            catch (Exception exc)
            {
                success = false;
                throw;
            }

            return success;
        }

        private bool StopListening()
        {
            bool success = true;

            try
            {
                _listener.Stop();
                _listener.MessageReceived -= OnMessage;
                _listener.ClientConnected -= OnClientConnected;
                _listener.ClientDisconnected -= OnClientDisconnected;
                _listener = null;
            }
            catch (Exception exc)
            {
                success = false;
                throw;
            }

            return success;
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (OnIncomingConnectionEstablished != null)
                OnIncomingConnectionEstablished(this, EventArgs.Empty);
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (OnIncomingConnectionTerminated != null)
                OnIncomingConnectionTerminated(this, EventArgs.Empty);
        }

        private void OnMessage(ITcpChannel channel, object message)
        {
            if (OnDataReceived != null)
                OnDataReceived(this, message);
        }

        public bool ToggleTransmission(bool enabled)
        {
            if (enabled)
                return StartListening();
            else
                return StartListening();
        }

        public int IncomingConnectionsPort
        {
            get { return _incomingConnectionsPort; }
            set { _incomingConnectionsPort = value; }
        }
    }
}
