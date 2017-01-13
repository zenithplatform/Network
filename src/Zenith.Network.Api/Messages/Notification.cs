using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Messages
{
    public class HubNotification : HubResponseMessageBase
    {
        public string Message { get; set; }
    }
}
