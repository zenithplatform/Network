using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenith.Core.Shared.EventAggregation;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class ServerEventHandlers : IEventTarget
    {
        MainViewModel _mainModel = null;

        public ServerEventHandlers(MainViewModel mainModel)
        {
            _mainModel = mainModel;
        }

        public void Register(IEventAggregator aggregator)
        {
            aggregator.Register<NodeConnectedEvent>(OnNodeConnected);
            aggregator.Register<NodeDisconnectedEvent>(OnNodeDisconnected);
            aggregator.Register<ServerStarted>(OnServerStarted);
            aggregator.Register<ServerStopped>(OnServerStopped);
            aggregator.Register<ServerError>(OnServerError);
        }

        public void OnNodeConnected(NodeConnectedEvent ev)
        {
            _mainModel.Connections.AddConnection(ev);

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                _mainModel.StatusBar.Message = string.Format("Active nodes : {0}.", _mainModel.Connections.ActiveNodesCount);

            }));
        }

        private void OnNodeDisconnected(NodeDisconnectedEvent ev)
        {
            _mainModel.Connections.UpdateConnectionStatus(ev.Id, Api.NodeStatus.Disconnected);

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                _mainModel.StatusBar.Message = string.Format("Active nodes : {0}.", _mainModel.Connections.ActiveNodesCount);

            }));
        }

        private void OnServerStarted(ServerStarted ev)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                _mainModel.StatusBar.CurrentStatus = "Running.";
                _mainModel.Info.StartUptimeCounter();

            }));
        }

        private void OnServerError(ServerError ev)
        {
            MessageBox.Show(ev.ToString());
        }

        private void OnServerStopped(ServerStopped ev)
        {
            _mainModel.Connections.ClearAllConnections();

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                _mainModel.StatusBar.CurrentStatus = "Stopped.";
                _mainModel.Info.StopUptimeCounter();

            }));
        }
    }
}
