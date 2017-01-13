using System.Net;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkSendStrategy
    {
        void Send<T>(T payload) where T : class;
    }
}
