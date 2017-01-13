using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Network.Api;
using Zenith.Network.ClientLibrary;

namespace Zenith.Network.TestClient
{
    public class Runner
    {
        public void Run(int numOfClients, int numOfRqPerClient)
        {
            string clientName = "";

            for (int i = 0; i < numOfClients; i++)
            {
                clientName = string.Format("Client {0}", i);
                Thread commThread = new Thread(new ParameterizedThreadStart(StartCommunication));
                commThread.Start(clientName);
            }
        }

        void StartCommunication(object clientName)
        {
            string client = clientName.ToString();
            bool result = false;
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 9999);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 9999);
            ClientWrapper wrapper = new ClientWrapper(endpoint);
            result = wrapper.Connect();

            if (result)
            {
                for (int i = 0; i < 100; i++)
                {
                    Send(client, wrapper);
                    Thread.Sleep(new Random().Next(1000, 5000));
                }
            }
        }

        void Send(object clientName, ClientWrapper wrapper)
        {
            if (wrapper.Connected)
            {
                string private_ip = "10.10.11.13";
                int private_port = 1071;

                string public_ip = "92.160.12.2";
                int public_port = 1072;

                NetworkNodeRegistrationRequest req = new NetworkNodeRegistrationRequest();
                //req.NodeMetadata = new NodeMetadata() { Name = clientName.ToString(), PrivateEndpoint = new IPEndPoint(IPAddress.Parse(private_ip), private_port), PublicEndpoint = new IPEndPoint(IPAddress.Parse(public_ip), public_port) };

                NetworkNodeRegistrationResponse res = wrapper.SendRequest(req) as NetworkNodeRegistrationResponse;
            }
        }
    }
}
