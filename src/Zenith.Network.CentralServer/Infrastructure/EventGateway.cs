using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared.EventAggregation;
using Zenith.Network.Api;
using Zenith.Network.Servers.Interfaces;

namespace Zenith.Network.Servers.Infrastructure
{
    public class ServerEventsGateway
    {
        private static object _syncLock = new object();
        private static IEventAggregator _aggregator = null;

        public static IEventAggregator Instance
        {
            get
            {
                if(_aggregator == null)
                {
                    lock(_syncLock)
                    {
                        if (_aggregator == null)
                            _aggregator = new EventAggregator();
                    }
                }

                return _aggregator;
            }
        }
    }

    //public class EventGateway
    //{
    //    public event EventHandler NodeConnected;

    //    private static readonly EventGateway _instance = new EventGateway();
    //    public static EventGateway Instance { get { return _instance; } }

    //    private EventGateway()
    //    {

    //    }

    //    public void FireEvent()
    //    {
    //        if (NodeConnected != null)
    //            NodeConnected(this, EventArgs.Empty);
    //    }
    //}

    //public class ServerEventsGateway : IServerEventGateway, IServerEventSource
    //{
    //    public event EventHandler<NodeMessageReceivedEventArgs> ClientMessageReceived;
    //    public event EventHandler<NodeActivityChangeArgs> NodeActivityChanged;
    //    public event EventHandler<NodeConnectedEventArgs> NodeConnected;
    //    public event EventHandler<NodeDisconnectedEventArgs> NodeDisconnected;
    //    public event EventHandler<NodeStatusChangedArgs> NodeStatusChanged;
    //    public event EventHandler<Exception> OnError;
    //    public event EventHandler OnNodeConnected;
    //    public event EventHandler ServerRestarted;
    //    public event EventHandler ServerRestarting;
    //    public event EventHandler ServerStarted;
    //    public event EventHandler ServerStarting;
    //    public event EventHandler ServerStopped;
    //    public event EventHandler ServerStopping;

    //    private static readonly ServerEventsGateway _instance = new ServerEventsGateway();
    //    public static ServerEventsGateway Instance { get { return _instance; } }

    //    private ServerEventsGateway()
    //    {

    //    }

    //    public void FireEvent(EventHandler handler)
    //    {
    //        if (handler != null)
    //            handler(this, EventArgs.Empty);
    //    }

    //    public void FireEvent<T>(EventHandler<T> handler, T args)
    //    {
    //        if (handler != null)
    //            handler(this, args);
    //    }
    //}

    //public interface IServerEventGateway
    //{
    //    void FireEvent(EventHandler handler);
    //    void FireEvent<T>(EventHandler<T> handler, T args);
    //}
}
