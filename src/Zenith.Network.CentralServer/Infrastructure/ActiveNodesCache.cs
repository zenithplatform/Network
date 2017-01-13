using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Api;

namespace Zenith.Network.Servers.Infrastructure
{
    public static class ActiveNodesCache
    {
        public static ConcurrentDictionary<string, NodeMetadata> ActiveNodes { get; set; }

        public static string GetConnectionId(string nodeId)
        {
            KeyValuePair<string, NodeMetadata> pair = ActiveNodes.SingleOrDefault(p => p.Value.Identifier
                                                                                              .NodeId
                                                                                              .Equals(nodeId));

            return pair.Key;
        }

        public static NodeMetadata GetMetadata(string nodeId)
        {
            KeyValuePair<string, NodeMetadata> pair = ActiveNodes.SingleOrDefault(p => p.Value.Identifier
                                                                                              .NodeId
                                                                                              .Equals(nodeId));

            return pair.Value;
        }
    }
}
