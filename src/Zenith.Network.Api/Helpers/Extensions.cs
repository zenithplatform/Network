using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Helpers
{
    public static class AddressExtensions
    {
        public static IPEndPoint ToIPEndPoint(this EndpointData data)
        {
            return new IPEndPoint(IPAddress.Parse(data.IPAddress), data.Port);
        }

        public static EndpointData ToEndPointData(this IPEndPoint endpoint)
        {
            EndpointData data = new EndpointData();
            data.IPAddress = endpoint.Address.ToString();
            data.Port = endpoint.Port;
            return data;
        }
    }
}
