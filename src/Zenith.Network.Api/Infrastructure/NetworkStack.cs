using System.Collections.Generic;

namespace Zenith.Network.Api.Infrastructure
{
    public class NetworkStack : INetworkStack
    {
        Dictionary<string, INetworkCoordinator> _coordinators = new Dictionary<string, INetworkCoordinator>();
        
        public void Add(string key, INetworkCoordinator coordinator)
        {
            _coordinators.Add(key, coordinator);
        }

        public INetworkCoordinator Get(string key)
        {
            return _coordinators[key];
        }

        public void Remove(string key)
        {
            _coordinators.Remove(key);
        }

        public IEnumerable<INetworkCoordinator> All
        {
            get
            {
                return _coordinators.Values;
            }
        }
    }
}
