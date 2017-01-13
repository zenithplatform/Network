using System;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkDiscovery
    {
        event EventHandler<NodeMetadata> NodeDiscovered;

        Task BeginDiscovery();
        void EndDiscovery();
    }
}
