using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api
{
    public class NetworkNodeResponseBase
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }
    }
}
