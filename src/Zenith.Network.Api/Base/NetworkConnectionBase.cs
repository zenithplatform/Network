using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api.Base
{
    public abstract class NetworkConnectorBase<T> : INetworkConnector<T> where T : class
    {
        IPEndPoint _localEndpoint, _remoteEndpoint = null;

        public NetworkConnectorBase(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            _localEndpoint = localEndpoint;
            _remoteEndpoint = remoteEndpoint;
        }

        public abstract void InitializeConnection();
        public abstract T GetConnection();
        public abstract bool IsConnected { get; }

        public virtual IPEndPoint LocalEndpoint
        {
            get { return _localEndpoint; }
            set { _localEndpoint = value; }
        }

        public virtual IPEndPoint RemoteEndpoint
        {
            get { return _remoteEndpoint; }
            set { _remoteEndpoint = value; }
        }
    }
}
