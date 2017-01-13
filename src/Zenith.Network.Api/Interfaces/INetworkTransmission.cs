using System;
using System.Net;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkTransmission
    {
        event EventHandler<object> OnDataReceived;
        event EventHandler<object> OnDataSent;
        event EventHandler OnIncomingConnectionEstablished;
        event EventHandler OnIncomingConnectionTerminated;

        bool ToggleTransmission(bool enabled);
        Task Send<T>(T data) where T : class;

        int IncomingConnectionsPort { get; set; }
    }
}
