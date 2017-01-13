namespace Zenith.Network.Api.Infrastructure
{
    public class NetworkConfig
    {
        public int IncomingConnectionsPort { get; set; }
        public int AutodiscoveryPort { get; set; }
        public byte[] NodeFingerprint { get; set; }
        public bool AcknowledgeBrodcast { get; set; }
        public int AnnounceInterval { get; set; }
        public string CentralServerAddress { get; set; }
        public string HubName { get; set; }
    }
}
