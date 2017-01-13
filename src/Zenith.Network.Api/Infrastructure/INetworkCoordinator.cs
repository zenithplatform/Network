using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api.Infrastructure
{
    public interface INetworkCoordinator : INetworkDiscovery, 
                                           INetworkRegistration, 
                                           INetworkTransmission
    {
        void Initialize(IComponentContainer container);
    }
}
