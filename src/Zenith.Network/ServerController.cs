using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager
{
    public class ServerController : IServerController
    {
        public IDisposable CentralHub { get; set; }
        const string ServerURI = "http://192.168.0.101:8080";

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void Restart(int? port)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            try
            {
                CentralHub = WebApp.Start(ServerURI);
                //ServerEventsGateway.Instance.Trigger(new ServerStarted());
            }
            catch (Exception exc)
            {
                //ServerError error = new ServerError();
                //error.ExceptionDetails = exc;
                //ServerEventsGateway.Instance.Trigger(error);
            }
        }

        public void Start(int? port)
        {
            StartOptions startOptions = new StartOptions(ServerURI);
            startOptions.Port = port;

            try
            {
                CentralHub = WebApp.Start(startOptions);
                //ServerEventsGateway.Instance.Trigger(new ServerStarted());
            }
            catch (Exception exc)
            {
                //ServerError error = new ServerError();
                //error.ExceptionDetails = exc;
                //ServerEventsGateway.Instance.Trigger(error);
            }
        }

        public void Stop()
        {
            CentralHub.Dispose();
            CentralHub = null;
        }
    }
}
