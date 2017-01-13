using System;
using Zenith.Core.DataProviders.Metadata;

namespace Zenith.Network.Api
{
    [DbName("test")]
    [TableName("nodes")]
    public class NodeMetadata
    {
        public NodeIdentifier Identifier { get; set; }
        public AddressingInfo AddressingInfo { get; set; }
        public DateTime TimeStamp { get; set; }

        public NodeMetadata()
        {

        }
    }

    public struct AddressingInfo
    {
        public EndpointData PublicEndpoint { get; set; }
        public EndpointData PrivateEndpoint { get; set; }
    }

    public struct EndpointData
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }

    public struct NodeIdentifier
    {
        public string Name { get; set; }
        [UniqueId("node_id")]
        public string NodeId { get; set; }
    }

    public enum NodeStatus
    {
        Connected,
        Disconnected,
        Unknown
    }

    public enum NodeActivity
    {
        Sending,
        Receiving,
        Idle,
        Closing
    }
}
