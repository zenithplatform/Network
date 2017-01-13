using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkRegistration
    {
        Task<bool> Register(NodeMetadata data);
        Task<bool> Unregister();
    }

    //public interface INetworkRegistration
    //{
    //    Task<bool> Register(RegistrationInfo data);
    //    Task<bool> Unregister();
    //}
}
