using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Zenith.Network.Api;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Zenith.Network.Tests
{
    [TestClass]
    public class Client
    {
        //[TestMethod]
        //public void SendRequest()
        //{
        //    bool result = false;
        //    IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 9999);
        //    ClientWrapper wrapper = new ClientWrapper(endpoint);

        //    result = wrapper.Connect();

        //    if (result)
        //    {
        //        string private_ip = "10.10.11.13";
        //        int private_port = 1071;

        //        string public_ip = "92.160.12.2";
        //        int public_port = 1072;

        //        NetworkNodeRegistrationRequest req = new NetworkNodeRegistrationRequest();
        //        //req.NodeMetadata = new NodeMetadata() { Name = "testnode1", PrivateEndpoint = new IPEndPoint(IPAddress.Parse(private_ip), private_port), PublicEndpoint = new IPEndPoint(IPAddress.Parse(public_ip), public_port) };

        //        NetworkNodeRegistrationResponse res = wrapper.SendRequest(req) as NetworkNodeRegistrationResponse;
        //    }
        //}

        [TestMethod]
        public void SignalRTest()
        {
            HubConnection connection = new HubConnection("http://localhost:8080/zenithnodehub");
            connection.Closed += Connection_Closed;
            IHubProxy hubProxy = connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            hubProxy.On<string, string>("AddMessage", (name, message) => OnMessage(name, message));
            try
            {
                connection.Start();
            }
            catch (Exception exc)
            {   
                //No connection: Don't enable Send button or show chat UI
                return;
            }
        }

        private void OnMessage(string name, string message)
        {

        }

        private void Connection_Closed()
        {

        }
    }
}
