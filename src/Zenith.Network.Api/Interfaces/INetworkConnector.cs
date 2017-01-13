using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkConnector<T> : INetworkConnector where T : class
    {
        T GetConnection();
    }

    public interface INetworkConnector
    {
        void InitializeConnection();
        bool IsConnected { get; }
    }
}
