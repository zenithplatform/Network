using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.ServerManager
{
    public interface IServerController
    {
        void Start();
        void Start(int? port);
        void Stop();
        void Restart();
        void Restart(int? port);
    }
}
