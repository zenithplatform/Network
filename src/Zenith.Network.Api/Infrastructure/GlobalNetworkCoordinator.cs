using Griffin.Net;
using Griffin.Net.Channels;
using Griffin.Net.Protocols;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Zenith.Core.Shared.Logging;
using Zenith.Network.Api.Messages;
using Zenith.Network.Api.Serialization;

namespace Zenith.Network.Api.Infrastructure
{
    public class GlobalNetworkCoordinator : HubClient, INetworkCoordinator
    {
        public event EventHandler<NodeMetadata> NodeDiscovered;
        public event EventHandler<object> OnDataReceived;
        public event EventHandler<object> OnDataSent;
        public event EventHandler OnIncomingConnectionEstablished;
        public event EventHandler OnIncomingConnectionTerminated;

        private int _incomingConnectionsPort = 0;
        NodeTcpListener _listener = null;
        NodeTcpClient _client = null;
        ChannelTcpListenerConfiguration _config = null;
        IComponentContainer _container = null;

        public void Initialize(IComponentContainer container)
        {
            _container = container;
        }

        public async Task BeginDiscovery()
        {
            List<NodeMetadata> result = null;
            QueryNodesResponse response = null;

            if (!Connected)
                return;

            try
            {
                QueryNodesRequest request = new QueryNodesRequest();
                request.All = true;
                //request.Identifiers = identifiers;

                response = await Call<QueryNodesResponse>("QueryNodes", new object[] { request });

                if (response == null)
                    return;

                result = response.NodesMetadata;

                foreach(NodeMetadata metadata in result)
                {
                    if (NodeDiscovered != null)
                        NodeDiscovered(this, metadata);
                }
            }
            catch (Exception exc)
            {
                result = null;
            }
        }

        public void EndDiscovery()
        {

        }

        public async Task<bool> Register(NodeMetadata data)
        {
            bool result = false;

            try
            {
                if (!Connected)
                    result = await Connect();

                if (!result)
                    return false;

                NodeRegistrationRequest request = new NodeRegistrationRequest();
                request.NodeMetadata = data;
                request.ConnectionId = this.ConnectionId;

                await Call("RegisterNode", new object[] { request });

                result = true;
            }
            catch (OperationCanceledException oce)
            {
                result = false;
                throw;
            }
            catch (HttpClientException httpExc)
            {
                result = false;
                throw;
            }
            catch (Exception exc)
            {
                result = false;
                throw;
            }

            return result;
        }

        public async Task<bool> Unregister()
        {
            bool result = true;

            if (!Connected)
                return false;

            NodeRegistrationRequest request = new NodeRegistrationRequest();
            request.ConnectionId = this.ConnectionId;
            request.Unregister = true;

            try
            {
                await Call("UnregisterNode", new object[] { request });
            }
            catch (Exception exc)
            {
                result = false;
            }

            return result;
        }

        protected override void PrepareConnection()
        {
            base.PrepareConnection();
            AddCallback<string>("Push", (message) => OnServerMessage(message));
        }

        private void OnServerMessage(string message)
        {
            try
            {
                var obj = MessageSerializer.Deserialize<HubResponseMessageBase>(message);
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task Send<T>(T data) where T : class
        {
            try
            {
                await _client.SendAsync(data);
            }
            catch (Exception exc)
            {
                _container.Get<IDefaultLogger>().Log(LogType.Error, exc.ToString());
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

        

        //public bool TurnOff()
        //{
        //    return StopListening();
        //}

        //public bool TurnOn()
        //{
        //    return StartListening();
        //}

        public int IncomingConnectionsPort
        {
            get { return _incomingConnectionsPort; }
            set { _incomingConnectionsPort = value; }
        }
    }
}
