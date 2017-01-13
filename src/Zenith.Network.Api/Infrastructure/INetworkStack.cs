using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Infrastructure
{
    public interface INetworkStack
    {
        void Add(string key, INetworkCoordinator coordinator);
        void Remove(string key);
        INetworkCoordinator Get(string key);

        IEnumerable<INetworkCoordinator> All { get; }
    }
}
