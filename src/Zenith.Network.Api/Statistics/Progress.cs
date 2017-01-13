using System;
using System.Threading;

namespace Zenith.Network.Api.Statistics
{
    public class HubProgress<NetworkJoinProgress> : IProgress<NetworkJoinProgress>
    {
        Action<NetworkJoinProgress> _action = null;

        public HubProgress(Action<NetworkJoinProgress> action)
        {
            _action = action;
        }

        public void Report(NetworkJoinProgress value)
        {
            _action(value);
            Thread.Sleep(1000);
        }
    }
}
