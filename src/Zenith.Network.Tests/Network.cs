using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Network.Api;
using Zenith.Network.Api.Infrastructure;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Client;

namespace Zenith.Network.Tests
{
    [TestClass]
    public class Network
    {
        [TestMethod]
        public void Run()
        {
            //NetworkConfig worldwideConfig = new NetworkConfig();
            //INetworkTransmission worldwideTransmission = new TcpNetworkTransmission(worldwideConfig);
            //INetworkCoordinator<NodeMetadata> worldwide = new WorldwideNetworkCoordinator(worldwideTransmission);

            //NetworkConfig localConfig = new NetworkConfig();
            //INetworkTransmission localTransmission = new TcpNetworkTransmission(localConfig);
            //INetworkCoordinator<NodeMetadata> local = new LocalNetworkCoordinator(localTransmission);

            //Node node = new Node();
            //node.AddCoordinator(worldwide);
            //node.AddCoordinator(local);

            //node.JoinNetwork(CancellationToken.None);

            //NetworkNode node = new NetworkNode();

            //INetworkProvider global = new GlobalNetworkProvider(new GlobalNetworkServiceContainer());
            //INetworkProvider local = new LocalNetworkProvider(new LocalNetworkServiceContainer());

            //node.NetworkProviders.Add("Global", global);
            //node.NetworkProviders.Add("Local", local);

            //node.Join();

            ZenithNetworkNode node = new ZenithNetworkNode();
            node.Initialize();
            node.Join();
        }
    }
}
