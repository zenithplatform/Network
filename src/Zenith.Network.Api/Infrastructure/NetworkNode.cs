using System;
using System.Net;
using System.Threading.Tasks;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Core.Nat;

namespace Zenith.Network.Api.Infrastructure
{
    public abstract class NetworkNode : INodeMetadata
    {
        NatUtils _natUtils = null;
        INetworkStartup _startup = null;
        INetworkStack _networkStack = null;

        public NetworkNode()
        {
            _natUtils = new NatUtils(true);

            _startup = new NetworkStartup();
            _startup.Initialize();

            _networkStack = _startup.Container.Get<INetworkStack>();
        }

        protected void Register(string key, Func<INetworkCoordinator> coordinatorActivation)
        {
            INetworkCoordinator coordinator = coordinatorActivation();
            coordinator.Initialize(this.Startup.Container);

            _networkStack.Add(key, coordinator);
        }

        public abstract void Initialize();

        public async Task<bool> Join()
        {
            foreach (INetworkCoordinator coordinator in _networkStack.All)
            {
                await coordinator.Register(this.GetMetadata());
                await coordinator.BeginDiscovery();
                coordinator.ToggleTransmission(true);
            }

            return true;
        }

        public async Task<bool> Leave()
        {
            foreach (INetworkCoordinator coordinator in _networkStack.All)
            {
                await coordinator.Unregister();
                coordinator.EndDiscovery();
                coordinator.ToggleTransmission(false);
            }

            return true;
        }

        public virtual NodeMetadata GetMetadata()
        {
            IPAddress external = IPAddress.Any;
            IPAddress local = IPAddress.Any;

            //Task.Factory.StartNew(async () => 
            //{
            //    external = await _natUtils.GetExternalIPAddress();
            //    local = await _natUtils.GetLocalIPAddress();

            //}).Wait();

            AddressingInfo addrInfo = new AddressingInfo();

            addrInfo.PrivateEndpoint = new EndpointData()
            {
                IPAddress = local.ToString(),
                Port = 9999
            };

            addrInfo.PublicEndpoint = new EndpointData()
            {
                IPAddress = external.ToString(),
                Port = 9999
            };

            NodeIdentifier identifier = new NodeIdentifier()
            {
                Name = Environment.MachineName,
                NodeId = Guid.NewGuid().ToString()
            };

            return new NodeMetadata()
            {
                AddressingInfo = addrInfo,
                Identifier = identifier,
                TimeStamp = DateTime.Now
            };
        }

        public INetworkStartup Startup
        {
            get { return _startup; }
        }
    }
}
