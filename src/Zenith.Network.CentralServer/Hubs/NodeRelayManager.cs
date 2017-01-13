using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api;

namespace Zenith.Network.Servers.Hubs
{
    public class NodeRelayManager
    {
        private Queue<NodeConnectionPair> _queue = null;

        public NodeRelayManager()
        {

        }

        public void StartCommunicationSession(string originConnectionId)
        {
            NodeConnectionPair pair = new NodeConnectionPair();
            pair.OriginId = originConnectionId;
            pair.TargetId = String.Empty;
            pair.IsPending = true;

            _queue.Enqueue(pair);
        }

        public void ConfirmCommunicationSession(string originConnectionId, string targetConnectionId)
        {
            NodeConnectionPair pair = _queue.SingleOrDefault(it => it.IsPending && it.OriginId.Equals(originConnectionId));

            if (pair != null)
            {
                pair.TargetId = targetConnectionId;
                pair.IsPending = false;
            }
        }

        public void AbandonCommunicationSession()
        {

        }

        public void StartRelaying(string originId, string targetId)
        {
            _queue.Enqueue(new NodeConnectionPair() { OriginId = originId, TargetId = targetId });
        }

        public NodeConnectionPair FinishRelaying()
        {
            return _queue.Dequeue();
        }
    }

    public class NodeConnectionPair
    {
        public string OriginId { get; set; }
        public string TargetId { get; set; }
        public bool IsPending { get; set; }
    }
}
